using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Dtos;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class ReadExpensesByResidenceIdUseCase : IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>>
{
    private readonly IExpenseRepository _expenseRepository;
    public ReadExpensesByResidenceIdUseCase(IExpenseRepository expenseRepository)
    {
        _expenseRepository  = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
    }
    public async Task<Result<IEnumerable<GetExpenseByIdOutput>>> ExecuteAsync(Guid residenceId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var expenses = await _expenseRepository.GetByPredicateAsync(x => x.ResidenceId == residenceId, cancellationToken);

        if (expenses is null)
        {
            return Result<IEnumerable<GetExpenseByIdOutput>>.Failure($"No expenses found for residence ID '{residenceId}'.", ErrorType.NotFound);
        }

        var output = Mapper.MapList<Expense, GetExpenseByIdOutput>(expenses);

        return Result<IEnumerable<GetExpenseByIdOutput>>.Success(output);
    }
}
