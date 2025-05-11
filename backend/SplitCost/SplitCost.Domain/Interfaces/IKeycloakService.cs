namespace SplitCost.Domain.Interfaces
{
    public interface IKeycloakService
    {
        Task<string> CreateUserAsync(string email, string userName, string password);
    }
}
