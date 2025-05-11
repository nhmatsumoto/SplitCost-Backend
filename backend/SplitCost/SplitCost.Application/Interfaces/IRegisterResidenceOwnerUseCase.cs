using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces
{
    public interface IRegisterResidenceOwnerUseCase
    {
        Task RegisterResidenceOwnerAsync(RegisterOwnerDto dto);
    }
}
