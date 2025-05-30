using Mapster;
using SplitCost.Application.Common;
using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases;

public class CreateExpenseUseCase : ICreateExpenseUseCase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IResidenceRepository _residenceRepository;
    private readonly IUserRepository _userRepository;

    public CreateExpenseUseCase(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork,
        IResidenceRepository residenceRepository,
        IUserRepository userRepository)
    {
        _expenseRepository      = expenseRepository     ?? throw new ArgumentNullException(nameof(expenseRepository));
        _unitOfWork             = unitOfWork            ?? throw new ArgumentNullException(nameof(unitOfWork));
        _residenceRepository    = residenceRepository   ?? throw new ArgumentNullException(nameof(residenceRepository));
        _userRepository         = userRepository        ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result> CreateExpense(CreateExpenseDto expenseDto)
    {
        //Map DTO To Domain Entity
        Expense expense = expenseDto.Adapt<Expense>();

        // Valida a entidade
        var residenceExists = await _residenceRepository.ExistsAsync(expense.ResidenceId);
        if (!residenceExists)
        {
            return Result.Failure($"Residence not found.", ErrorType.NotFound);
        }

        var payingUserExists = await _userRepository.ExistsAsync(expense.PaidByUserId);
        if (!payingUserExists)
        {
            return Result.Failure($"Paying user not found.", ErrorType.NotFound);
        }

        var registeredUserExists = await _userRepository.ExistsAsync(expense.RegisteredByUserId);
        if (!registeredUserExists)
        {
            return Result.Failure($"Registered user not found.", ErrorType.NotFound);
        }

        // Adiciona ao repositório e salva as mudanças
        await _expenseRepository.AddAsync(expense);
        await _unitOfWork.SaveChangesAsync();

        var expenseDtoResult = new ExpenseDto
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

        return Result.Success(expenseDtoResult);
    }

}
