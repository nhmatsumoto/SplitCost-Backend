using Microsoft.Extensions.Configuration;
using SplitCost.Domain.Exceptions;
using SplitCost.Domain.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SplitCost.Infrastructure.Services;

public class KeycloakService : IKeycloakService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public KeycloakService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> CreateUserAsync(string username, string firstName, string lastName, string email, string password)
    {
        var token = await GetApplicationAdminTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var realm = _config["Keycloak:Realm"];
        var createUserUrl = $"{_config["Keycloak:BaseUrl"]}/admin/realms/{realm}/users";

        var userPayload = new
        {
            username = email,
            email = email,
            firstName = firstName,
            lastName = lastName,
            enabled = true,
            emailVerified = true,
            credentials = new[]
            {
            new {
                type = "password",
                value = password,
                temporary = false
            }
        }
        };

        var content = new StringContent(JsonSerializer.Serialize(userPayload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(createUserUrl, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var locationHeader = response.Headers.Location?.ToString();
            if (string.IsNullOrWhiteSpace(locationHeader))
            {
                throw new KeycloakUserCreationFailedException("Usuário criado, mas ID não retornado.");
            }
            var userId = locationHeader.Split('/').Last();
            return userId;
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

    private async Task<string> GetApplicationAdminTokenAsync()
    {
        using var client = new HttpClient();
        
        var tokenEndpoint = _config["Keycloak:TokenEndpoint"];

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", _config["Keycloak:ClientId"]),
            new KeyValuePair<string, string>("client_secret", _config["Keycloak:ClientSecret"]),
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var response = await client.PostAsync(tokenEndpoint, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Erro ao obter token do Keycloak: {responseBody}");

        var tokenObj = JsonSerializer.Deserialize<JsonElement>(responseBody);
        return tokenObj.GetProperty("access_token").GetString();
    }
}
