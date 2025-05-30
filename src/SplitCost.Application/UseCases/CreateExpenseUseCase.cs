using Mapster;
using MapsterMapper;
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
    private readonly IMapper _mapper;

    public CreateExpenseUseCase(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork,
        IResidenceRepository residenceRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _expenseRepository      = expenseRepository     ?? throw new ArgumentNullException(nameof(expenseRepository));
        _unitOfWork             = unitOfWork            ?? throw new ArgumentNullException(nameof(unitOfWork));
        _residenceRepository    = residenceRepository   ?? throw new ArgumentNullException(nameof(residenceRepository));
        _userRepository         = userRepository        ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper                 = mapper                ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<ExpenseDto>> CreateExpense(CreateExpenseDto expenseDto)
    {
        //Map DTO To Domain Entity
        var expense = _mapper.Map<Expense>(expenseDto);

        // Valida a entidade
        var residenceExists = await _residenceRepository.ExistsAsync(expense.ResidenceId);
        if (!residenceExists)
        {
            return Result<ExpenseDto>.Failure($"Residence not found.", ErrorType.NotFound);
        }

        var payingUserExists = await _userRepository.ExistsAsync(expense.PaidByUserId);
        if (!payingUserExists)
        {
            return Result<ExpenseDto>.Failure($"Paying user not found.", ErrorType.NotFound);
        }

        var registeredUserExists = await _userRepository.ExistsAsync(expense.RegisteredByUserId);
        if (!registeredUserExists)
        {
            return Result<ExpenseDto>.Failure($"Registered user not found.", ErrorType.NotFound);
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
            RegisteredByUserId = expense.RegisteredByUserId,
            ResidenceId = expense.ResidenceId,
            Type = expense.Type
        };

        return Result<ExpenseDto>.Success(expenseDtoResult);
    }

}
