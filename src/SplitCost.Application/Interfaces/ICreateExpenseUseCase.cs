using SplitCost.Application.Common;
using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces;

public interface ICreateExpenseUseCase
{
    Task<Result> CreateExpense(CreateExpenseDto expenseDto);
}
