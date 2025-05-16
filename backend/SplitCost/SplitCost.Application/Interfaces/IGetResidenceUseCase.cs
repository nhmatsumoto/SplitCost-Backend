using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces
{
    public interface IGetResidenceUseCase
    {
        Task<ResidenceDto?> GetByIdAsync(Guid id);
        Task<ResidenceDto?> GetByUserIdAsync(Guid id);
        Task<IEnumerable<ResidenceDto>> GetAllAsync();
    }
}
