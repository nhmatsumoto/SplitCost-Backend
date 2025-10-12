using Azure.Core;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace SplitCost.Infrastructure.Services;

public class KeycloakUserService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public KeycloakUserService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    private async Task<string> GetAdminTokenAsync()
    {
        var tokenUrl = $"{_config["Keycloak:AuthServerUrl"]}/realms/{_config["Keycloak:Realm"]}/protocol/openid-connect/token";

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _config["Keycloak:ClientId"],
            ["client_secret"] = _config["Keycloak:ClientSecret"],
        });

        var response = await _httpClient.PostAsync(tokenUrl, content);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("access_token").GetString()!;
    }

    public async Task AssignRoleAsync(string userId, string roleName)
    {
        var token = await GetAdminTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // 1️⃣ Obter role
        var roleUrl = $"{_config["Keycloak:AuthServerUrl"]}/admin/realms/{_config["Keycloak:Realm"]}/clients";
        var clients = await _httpClient.GetFromJsonAsync<JsonElement[]>(roleUrl);
        var client = clients.FirstOrDefault(c => c.GetProperty("clientId").GetString() == _config["Keycloak:ClientId"]);
        var clientId = client.GetProperty("id").GetString();

        var roleResponse = await _httpClient.GetAsync($"{roleUrl}/{clientId}/roles/{roleName}");
        var role = await roleResponse.Content.ReadFromJsonAsync<JsonElement>();

        // 2️⃣ Atribuir ao usuário
        var assignUrl = $"{_config["Keycloak:AuthServerUrl"]}/admin/realms/{_config["Keycloak:Realm"]}/users/{userId}/role-mappings/clients/{clientId}";
        await _httpClient.PostAsJsonAsync(assignUrl, new[] { role });
    }
}


// Apos criar o usuário:

//await _keycloakUserService.AssignRoleAsync(createdUserId, request.Profile == "Owner" ? "Owner" : "Member");
