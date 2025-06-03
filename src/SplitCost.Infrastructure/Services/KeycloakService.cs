using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SplitCost.Application.Common.Services;
using SplitCost.Domain.Exceptions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SplitCost.Infrastructure.Services;

public class KeycloakService : IKeycloakService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly KeycloakSettings _settings;

    public KeycloakService(HttpClient httpClient, IConfiguration config, IOptions<KeycloakSettings> options)
    {
        _httpClient     = httpClient    ?? throw new ArgumentNullException(nameof(httpClient));
        _config         = config        ?? throw new ArgumentNullException(nameof(config));
        _settings       = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<Guid> CreateUserAsync(string username, string firstName, string lastName, string email, string password, CancellationToken cancellationToken)
    {
        var token = await GetApplicationAdminTokenAsync(cancellationToken);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createUserUrl = $"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users";

        var userPayload = new KeycloakUserPayload
        {
            username = username,
            email = email,
            firstName = firstName,
            lastName = lastName,
            credentials = new[]
            {
                new Credential { value = password }
            }
        };

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var content = new StringContent(
            JsonSerializer.Serialize(userPayload, jsonOptions),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(createUserUrl, content, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var locationHeader = response.Headers.Location?.ToString();
            if (string.IsNullOrWhiteSpace(locationHeader))
            {
                throw new KeycloakUserCreationFailedException("Usuário criado, mas ID não retornado.");
            }
            var userId = locationHeader.Split('/').Last();
            
            return Guid.Parse(userId);
        }

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            throw new KeycloakUserConflictException("O Email informado não é valido");
        }

        var exceptionMap = new Dictionary<HttpStatusCode, Func<string, string, KeycloakApiException>>
        {
            { HttpStatusCode.Conflict, (message, content) => new KeycloakUserConflictException($"Erro ao criar usuário: Conflito. Detalhes: {content}", content) },
            { HttpStatusCode.BadRequest, (message, content) => new KeycloakBadRequestException($"Erro ao criar usuário: Requisição inválida. Detalhes: {content}", content) },
            { HttpStatusCode.Unauthorized, (message, content) => new KeycloakUnauthorizedException($"Erro ao criar usuário: Não autorizado. Detalhes: {content}", content) },
            { HttpStatusCode.Forbidden, (message, content) => new KeycloakForbiddenException($"Erro ao criar usuário: Proibido. Detalhes: {content}", content) }
        };

        if (exceptionMap.TryGetValue(response.StatusCode, out var exceptionFactory))
        {
            throw exceptionFactory($"Erro ao criar usuário: {response.StatusCode}", responseContent);
        }
        else
        {
            throw new KeycloakUserCreationFailedException($"Erro ao criar usuário no Keycloak: {response.StatusCode} - {response.ReasonPhrase}. Detalhes: {responseContent}", responseContent);
        }
    }

    private async Task<string> GetApplicationAdminTokenAsync(CancellationToken cancellationToken)
    {
        var tokenEndpoint = _settings.GetTokenEndpoint();

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", _settings.ClientId),
            new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var response = await _httpClient.PostAsync(tokenEndpoint, content, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Erro ao obter token do Keycloak: {responseBody}");

        var tokenObj = JsonSerializer.Deserialize<JsonElement>(responseBody);

        if (!tokenObj.TryGetProperty("access_token", out var accessTokenProp) || string.IsNullOrWhiteSpace(accessTokenProp.GetString()))
        {
            throw new Exception("Resposta do Keycloak não contém um access_token válido.");
        }

        return accessTokenProp.GetString()!;
    }
}
