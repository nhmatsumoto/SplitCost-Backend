namespace SplitCost.Application.Common.Interfaces.Identity;

public interface IKeycloakAuthService
{
    Task<string> GetAdminTokenAsync(CancellationToken cancellationToken);
}
