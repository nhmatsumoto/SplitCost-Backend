using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces
{
    public interface IAppUserUseCase
    {
        Task<Guid> RegisterUserAsync(RegisterUserDto registerUserDto);
    }
}
