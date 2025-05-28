using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces
{
    public interface IUpdateResidenceUseCase
    {
        Task<ResidenceDto> UpdateResidenceAsync(Guid residenceId, UpdateResidenceDto residenceDto);
    }
}
