using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces;

public interface IReadExpenseUseCase
{
    Task<ExpenseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ExpenseDto>> GetByResidenceIdAsync(Guid residenceId);
}
