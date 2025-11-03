using Microsoft.Extensions.Options;
using SplitCost.Application.Common.Interfaces.Identity;
using SplitCost.Domain.Exceptions.Keycloak;
using SplitCost.Infrastructure.Services.Auth;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SplitCost.Infrastructure.Identity.Keycloak;

public class KeycloakUserService : IKeycloakUserService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly KeycloakSettings _settings;
    private readonly IKeycloakAuthService _authService;

    public KeycloakUserService(HttpClient httpClient, IOptions<KeycloakSettings> options, IKeycloakAuthService authService, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task<Guid> CreateUserAsync(string username, string firstName, string lastName, string email, string password, CancellationToken cancellationToken)
    {
        var token = await _authService.GetAdminTokenAsync(cancellationToken);
        //var client = _httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var payload = new
        {
            username,
            email,
            firstName,
            lastName,
            enabled = true,
            credentials = new[]
            {
                new
                {
                    type = "password",
                    value = password,
                    temporary = false
                }
            }
        };

        var url = $"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users";
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var locationHeader = response.Headers.Location?.ToString();
            if (string.IsNullOrWhiteSpace(locationHeader))
                throw new KeycloakUserCreationFailedException("Usuário criado, mas ID não retornado.", responseContent);

            var userId = locationHeader.Split('/').Last();
            return Guid.Parse(userId);
        }

        HandleError(response.StatusCode, "Erro ao criar usuário", responseContent);
        return Guid.Empty;
    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var token = await _authService.GetAdminTokenAsync(cancellationToken);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var url = $"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users/{userId}";
        var response = await _httpClient.DeleteAsync(url, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
            return;

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new KeycloakUserNotFoundException($"Usuário {userId} não encontrado.");

        HandleError(response.StatusCode, "Erro ao deletar usuário", responseContent);
    }

    public async Task AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken)
    {
        var token = await _authService.GetAdminTokenAsync(cancellationToken);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Buscar role
        var roleUrl = $"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/roles/{roleName}";
        var roleResponse = await _httpClient.GetAsync(roleUrl, cancellationToken);
        var roleContent = await roleResponse.Content.ReadAsStringAsync(cancellationToken);

        if (!roleResponse.IsSuccessStatusCode)
            throw new Exception($"Role {roleName} não encontrada. Detalhes: {roleContent}");

        var role = JsonSerializer.Deserialize<KeycloakRole>(roleContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //Atribuir role ao usuário
        var assignUrl = $"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users/{userId}/role-mappings/realm";
        var assignContent = new StringContent(JsonSerializer.Serialize(new[] { role }), Encoding.UTF8, "application/json");

        var assignResponse = await _httpClient.PostAsync(assignUrl, assignContent, cancellationToken);
        var assignResponseContent = await assignResponse.Content.ReadAsStringAsync(cancellationToken);

        if (!assignResponse.IsSuccessStatusCode)
            HandleError(assignResponse.StatusCode, $"Erro ao atribuir role {roleName}", assignResponseContent);
    }

    private static void HandleError(HttpStatusCode status, string message, string details)
    {
        throw status switch
        {
            HttpStatusCode.Conflict => new KeycloakUserConflictException(message, details),
            HttpStatusCode.BadRequest => new KeycloakBadRequestException(message, details),
            HttpStatusCode.Unauthorized => new KeycloakUnauthorizedException(message, details),
            HttpStatusCode.Forbidden => new KeycloakForbiddenException(message, details),
            _ => new KeycloakInternalServerErrorException($"{message}: {status}", details)
        };
    }

    private class KeycloakRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
