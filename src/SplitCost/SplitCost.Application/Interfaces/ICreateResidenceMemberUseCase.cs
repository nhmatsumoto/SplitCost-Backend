using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces
{
    public interface ICreateResidenceMemberUseCase
    {
        Task RegisterResidenceMemberAsync(CreateResidenceMemberDto dto);
    }
}
