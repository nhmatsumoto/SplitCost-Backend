using System.ComponentModel.DataAnnotations;

namespace SplitCost.Infrastructure.Services;

public class KeycloakSettings
{
    [Required]
    public string Realm { get; set; } = string.Empty;

    [Required]
    public string BaseUrl { get; set; } = string.Empty;

    [Required]
    public string ClientId { get; set; } = string.Empty;

    [Required]
    public string ClientSecret { get; set; } = string.Empty;

    [Required]
    public string TokenEndpoint { get; set; } = string.Empty;


    public string GetTokenEndpoint()
    {
        if (string.IsNullOrWhiteSpace(BaseUrl) || string.IsNullOrWhiteSpace(Realm))
            throw new InvalidOperationException("BaseUrl ou Realm não foram configurados corretamente.");

        return $"{BaseUrl}/realms/{Realm}/protocol/openid-connect/token";
    }
}
