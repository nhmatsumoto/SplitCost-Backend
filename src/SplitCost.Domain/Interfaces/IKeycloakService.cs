namespace SplitCost.Domain.Interfaces
{
    public interface IKeycloakService
    {
        Task<Guid> CreateUserAsync(string username, string firstName, string lastName, string email, string password, CancellationToken cancellationToken);
    }
}
