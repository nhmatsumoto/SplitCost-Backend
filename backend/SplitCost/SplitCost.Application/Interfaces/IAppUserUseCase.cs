namespace SplitCost.Application.Interfaces
{
    public interface IAppUserUseCase
    {
        Task<Guid> RegisterUserAsync(string nome, string email, string senha);
    }
}
