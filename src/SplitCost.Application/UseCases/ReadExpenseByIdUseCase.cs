using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Dtos;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class ReadExpenseByIdUseCase : IUseCase<Guid, Result<GetExpenseByIdOutput>>
{
    private readonly IExpenseRepository _expenseRepository;
   
    public ReadExpenseByIdUseCase(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository ?? throw new ArgumentException(nameof(expenseRepository));
    }

    public async Task<Result<GetExpenseByIdOutput>> ExecuteAsync(Guid input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var expense = await _expenseRepository.GetByIdAsync(input, cancellationToken);

        if (expense == null)
        {
            return Result<GetExpenseByIdOutput>.Failure($"Expense not found.", ErrorType.NotFound);
        }

        var expenseDto = Mapper.Map<Expense, GetExpenseByIdOutput>(expense);

        return Result<GetExpenseByIdOutput>.Success(expenseDto);
    }
}
