using SplitCost.Application.Common;

namespace SplitCost.Application.Interfaces;

public interface IReadMemberUseCase
{
    Task<Result> GetByResidenceIdAsync(Guid residenceId);
}
