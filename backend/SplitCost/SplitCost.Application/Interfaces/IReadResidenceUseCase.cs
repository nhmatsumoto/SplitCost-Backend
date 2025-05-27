using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces
{
    public interface IReadResidenceUseCase
    {
        Task<ResidenceDto?> GetByIdAsync(Guid id);
        Task<ResidenceDto?> GetByUserIdAsync(Guid id);
        Task<IEnumerable<ResidenceDto>> GetAllAsync();

        Task<Boolean> UserHasResidence(Guid userId);
    }
}
