using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace SplitCost.Infrastructure.Services.Auth;

public class KeycloakUserPayload
{
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
    public bool enabled { get; set; } = true;
    public bool emailVerified { get; set; } = true;
    public Credential[] credentials { get; set; } = Array.Empty<Credential>();
}
