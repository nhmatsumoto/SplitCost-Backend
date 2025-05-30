using SplitCost.Application.Common;
using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces
{
    public interface ICreateResidenceUseCase
    {
        Task<Result<ResidenceDto>> CreateResidenceAsync(CreateResidenceDto dto, Guid userId);
    }
}
