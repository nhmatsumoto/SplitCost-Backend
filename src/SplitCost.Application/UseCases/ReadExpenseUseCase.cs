using SplitCost.Application.Common;
using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases;

public class ReadExpenseUseCase : IReadExpenseUseCase
{
    private readonly IExpenseRepository _expenseRepository;

    public ReadExpenseUseCase(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository ?? throw new ArgumentException(nameof(expenseRepository));
    }

    public async Task<Result> GetByIdAsync(Guid id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense == null)
        {
            return Result.Failure($"Expense not found.", ErrorType.NotFound);
        }

        var expenseDto = new ExpenseDto
        {
            Id = expense.Id,
            Category = expense.Category,
            Amount = expense.Amount,
            Date = expense.Date,
            Description = expense.Description,
            IsSharedAmongMembers = expense.IsSharedAmongMembers,
            PaidByUserId = expense.PaidByUserId,
            RegisterByUserId = expense.RegisteredByUserId,
            ResidenceId = expense.ResidenceId,
            Type = expense.Type
        };

        return Result.Success(expenseDto);
    }

    public async Task<Result> GetByResidenceIdAsync(Guid residenceId)
    {
        var expenses = await _expenseRepository.GetByResidenceIdAsync(residenceId);

        if (expenses == null || !expenses.Any())
        {
            return Result.Failure($"No expenses found for residence ID '{residenceId}'.", ErrorType.NotFound);
        }

        var expenseDtos = expenses.Select(expense => new ExpenseDto
        {
            Id = expense.Id,
            Category = expense.Category,
            Amount = expense.Amount,
            Date = expense.Date,
            Description = expense.Description,
            IsSharedAmongMembers = expense.IsSharedAmongMembers,
            PaidByUserId = expense.PaidByUserId,
            RegisterByUserId = expense.RegisteredByUserId,
            ResidenceId = expense.ResidenceId,
            Type = expense.Type
        });

        return Result.Success(expenseDtos);
    }
}
