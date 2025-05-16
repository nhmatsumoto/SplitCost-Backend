namespace SplitCost.Domain.Interfaces
{
    public interface IKeycloakService
    {
        Task<string> CreateUserAsync(string username, string firstName, string lastName, string email, string password);
    }
}
