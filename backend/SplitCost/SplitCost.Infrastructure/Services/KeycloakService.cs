using Microsoft.Extensions.Configuration;
using SplitCost.Domain.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SplitCost.Infrastructure.Services
{
    public class KeycloakService : IKeycloakService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public KeycloakService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> CreateUserAsync(string email, string nome, string senha)
        {
            var token = await GetAdminTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var realm = _config["Keycloak:Realm"];
            var createUserUrl = $"{_config["Keycloak:BaseUrl"]}/admin/realms/{realm}/users";

            var userPayload = new
            {
                username = email,
                email = email,
                firstName = nome,
                enabled = true,
                credentials = new[]
                {
                    new {
                        type = "password",
                        value = senha,
                        temporary = false
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(userPayload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(createUserUrl, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao criar usuário no Keycloak: {response.StatusCode}");

            // Obter o ID do usuário no header `Location`
            var locationHeader = response.Headers.Location?.ToString();
            if (string.IsNullOrWhiteSpace(locationHeader))
                throw new Exception("Usuário criado, mas ID não retornado.");

            var userId = locationHeader.Split('/').Last();
            return userId;
        }

        private async Task<string> GetAdminTokenAsync()
        {
            using var client = new HttpClient();

            var tokenEndpoint = "http://localhost:8080/realms/split-costs/protocol/openid-connect/token";

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "split-costs-client"),
                new KeyValuePair<string, string>("client_secret", "0p9aqmjYx501YZiOXW940QDoCHxa7Nbp"),
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

}
