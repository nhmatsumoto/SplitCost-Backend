namespace SplitCost.Application.Common.Interfaces.Identity;

public interface IKeycloakUserService
{
    Task<Guid> CreateUserAsync(string username, string firstName, string lastName, string email, string password, CancellationToken cancellationToken);
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
    Task AssignRoleAsync(string userId, string roleName, CancellationToken cancellationToken);
}
