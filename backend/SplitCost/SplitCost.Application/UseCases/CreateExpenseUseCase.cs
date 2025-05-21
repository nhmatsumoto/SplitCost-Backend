using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases;

public class CreateExpenseUseCase : ICreateExpenseUseCase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateExpenseUseCase(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ExpenseDto> CreateExpense(CreateExpenseDto expenseDto)
    {
        var expense = new Expense(
            expenseDto.Type,
            expenseDto.Category,
            expenseDto.Amount,
            expenseDto.Date,
            expenseDto.IsSharedAmongMembers,
            expenseDto.Description,
            expenseDto.ResidenceId,
            expenseDto.PaidByUserId,
            expenseDto.RegisterByUserId
        );

        await _expenseRepository.AddAsync(expense);

        await _unitOfWork.SaveChangesAsync();

        return new ExpenseDto
        {
            Amount = expense.Amount,
            Category = expense.Category,
            Date = expense.Date,
            Description = expense.Description,
            Id = expense.Id,
            IsSharedAmongMembers = expense.IsSharedAmongMembers,
            PaidByUserId = expense.PaidByUserId,
            RegisterByUserId = expense.RegisteredByUserId,
            ResidenceId = expense.ResidenceId,
            Type = expense.Type
        };
    }
    
}
