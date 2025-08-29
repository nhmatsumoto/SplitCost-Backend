namespace SplitCost.Application.Common.Services;

public interface IKeycloakService
{
    Task<Guid> CreateUserAsync(string username, string firstName, string lastName, string email, string password, CancellationToken cancellationToken);
    //Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
}
