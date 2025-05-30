using SplitCost.Application.Common;

namespace SplitCost.Application.Interfaces;

public interface IReadExpenseUseCase
{
    Task<Result> GetByIdAsync(Guid id);
    Task<Result> GetByResidenceIdAsync(Guid residenceId);
}
