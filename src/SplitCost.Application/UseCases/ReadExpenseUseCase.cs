using MapsterMapper;
using SplitCost.Application.Common;
using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases;

public class ReadExpenseUseCase : IReadExpenseUseCase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public ReadExpenseUseCase(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository ?? throw new ArgumentException(nameof(expenseRepository));
        _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
    }

    public async Task<Result<ExpenseDto>> GetByIdAsync(Guid id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);

        if (expense == null)
        {
            return Result<ExpenseDto>.Failure($"Expense not found.", ErrorType.NotFound);
        }

        var expenseDto = _mapper.Map<ExpenseDto>(expense);

        return Result<ExpenseDto>.Success(expenseDto);
    }

    public async Task<Result<IEnumerable<ExpenseDto>>> GetExpensesByResidenceIdAsync(Guid residenceId)
    {
        var expenses = await _expenseRepository.GetByResidenceIdAsync(residenceId);

        if (expenses == null || !expenses.Any())
        {
            return Result<IEnumerable<ExpenseDto>>.Failure($"No expenses found for residence ID '{residenceId}'.", ErrorType.NotFound);
        }

        var expenseDtos = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);   

        return Result<IEnumerable<ExpenseDto>>.Success(expenseDtos);
    }
}
