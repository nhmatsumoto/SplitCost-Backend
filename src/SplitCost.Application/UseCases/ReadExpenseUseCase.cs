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

    public async Task<ExpenseDto?> GetByIdAsync(Guid id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense == null) return null;
        return new ExpenseDto
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
    }

    public async Task<IEnumerable<ExpenseDto>> GetByResidenceIdAsync(Guid residenceId)
    {
        var expenses = await _expenseRepository.GetByResidenceIdAsync(residenceId);
        return expenses.Select(e => new ExpenseDto
        {
            Id = e.Id,
            Category = e.Category,
            Amount = e.Amount,
            Date = e.Date,
            Description = e.Description,
            IsSharedAmongMembers = e.IsSharedAmongMembers,
            PaidByUserId = e.PaidByUserId,
            RegisterByUserId = e.RegisteredByUserId,
            ResidenceId = e.ResidenceId,
            Type = e.Type
        });
    }
}
