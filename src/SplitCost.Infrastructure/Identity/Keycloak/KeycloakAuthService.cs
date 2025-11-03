using Microsoft.Extensions.Options;
using SplitCost.Application.Common.Interfaces.Identity;
using SplitCost.Domain.Exceptions.Keycloak;
using SplitCost.Infrastructure.Services.Auth;
using System.Text.Json;

namespace SplitCost.Infrastructure.Identity.Keycloak;

public class KeycloakAuthService : IKeycloakAuthService
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakSettings _settings;

    public KeycloakAuthService(HttpClient httpClient, IOptions<KeycloakSettings> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<string> GetAdminTokenAsync(CancellationToken cancellationToken)
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

}
