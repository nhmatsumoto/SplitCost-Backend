using Microsoft.Extensions.Options;
using SplitCost.Application.Common.Services;
using SplitCost.Domain.Exceptions.Keycloak;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SplitCost.Infrastructure.Services;

public class KeycloakService : IKeycloakService
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakSettings _settings;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly Dictionary<HttpStatusCode, Func<string, string, KeycloakException>> _exceptionMap =
        new()
        {
            { HttpStatusCode.Conflict, (msg, content) => new KeycloakUserConflictException(msg, content) },
            { HttpStatusCode.BadRequest, (msg, content) => new KeycloakBadRequestException(msg, content) },
            { HttpStatusCode.Unauthorized, (msg, content) => new KeycloakUnauthorizedException(msg, content) },
            { HttpStatusCode.Forbidden, (msg, content) => new KeycloakForbiddenException(msg, content) }
        };

    public KeycloakService(HttpClient httpClient, IOptions<KeycloakSettings> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<Guid> CreateUserAsync(
        string username, string firstName, string lastName, string email, string password,
        CancellationToken cancellationToken)
    {
        await SetAuthorizationHeaderAsync(cancellationToken);

        var createUserUrl = $"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users";
        var payload = new KeycloakUserPayload
        {
            username = username,
            email = email,
            firstName = firstName,
            lastName = lastName,
            credentials = new[] { new Credential { value = password } }
        };

        var content = new StringContent(JsonSerializer.Serialize(payload, JsonOptions), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(createUserUrl, content, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var locationHeader = response.Headers.Location?.ToString();
            if (string.IsNullOrWhiteSpace(locationHeader))
                throw new KeycloakUserCreationFailedException("Usuário criado, mas ID não retornado.", responseContent);

            var userId = locationHeader.Split('/').Last();
            return Guid.Parse(userId);
        }

        ThrowMappedException(response.StatusCode, "Erro ao criar usuário", responseContent);
        return Guid.Empty; // Nunca alcançado
    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        await SetAuthorizationHeaderAsync(cancellationToken);

        var deleteUserUrl = $"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users/{userId}";
        var response = await _httpClient.DeleteAsync(deleteUserUrl, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode) return;

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new KeycloakUserNotFoundException($"Usuário {userId} não encontrado no Keycloak.");
        }

        ThrowMappedException(response.StatusCode, $"Erro ao deletar usuário {userId}", responseContent);
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
            throw new KeycloakInternalServerErrorException($"Erro ao obter token do Keycloak: {responseBody}", responseBody);

        var tokenObj = JsonSerializer.Deserialize<JsonElement>(responseBody);
        if (!tokenObj.TryGetProperty("access_token", out var tokenProp) || string.IsNullOrWhiteSpace(tokenProp.GetString()))
            throw new KeycloakUnauthorizedException("Resposta do Keycloak não contém um access_token válido.", responseBody);

        return tokenProp.GetString()!;
    }

    private async Task SetAuthorizationHeaderAsync(CancellationToken cancellationToken)
    {
        var token = await GetApplicationAdminTokenAsync(cancellationToken);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ThrowMappedException(HttpStatusCode statusCode, string message, string details)
    {
        if (_exceptionMap.TryGetValue(statusCode, out var factory))
            throw factory(message, details);

        throw new KeycloakUserCreationFailedException($"{message}: {statusCode}. Detalhes: {details}", details);
    }
}
