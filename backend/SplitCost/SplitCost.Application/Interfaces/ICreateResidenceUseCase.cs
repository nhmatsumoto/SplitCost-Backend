using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces
{
    public interface ICreateResidenceUseCase
    {
        Task<ResidenceDto> CreateResidenceAsync(CreateResidenceDto dto, Guid userId);
    }
}
