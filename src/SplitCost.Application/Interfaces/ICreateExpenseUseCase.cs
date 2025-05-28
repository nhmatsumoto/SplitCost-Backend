using SplitCost.Application.DTOs;

namespace SplitCost.Application.Interfaces;

public interface ICreateExpenseUseCase
{
    Task<ExpenseDto> CreateExpense(CreateExpenseDto expenseDto);
}
