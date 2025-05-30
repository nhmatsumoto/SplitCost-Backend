using SplitCost.Application.Common;
using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces;

public interface IReadExpenseUseCase
{
    Task<Result<ExpenseDto>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<ExpenseDto>>> GetExpensesByResidenceIdAsync(Guid residenceId);
}
