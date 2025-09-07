using MapsterMapper;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Dtos;

namespace SplitCost.Application.UseCases;

public class ReadExpensesByResidenceIdUseCase : IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>>
{
    private readonly IExpenseRepository _expenseRepository;
    public ReadExpensesByResidenceIdUseCase(IExpenseRepository expenseRepository)
    {
        _expenseRepository  = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
    }
    public async Task<Result<IEnumerable<GetExpenseByIdOutput>>> ExecuteAsync(Guid input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();


        var expense = await _expenseRepository.GetByIdAsync(input, cancellationToken);

        if (expense is null)
        {
            return Result<IEnumerable<GetExpenseByIdOutput>>.Failure($"No expenses found for residence ID '{input}'.", ErrorType.NotFound);
        }

        var output = _mapper.Map<IEnumerable<GetExpenseByIdOutput>>(expense);

        return Result<IEnumerable<GetExpenseByIdOutput>>.Success(output);
    }
}
