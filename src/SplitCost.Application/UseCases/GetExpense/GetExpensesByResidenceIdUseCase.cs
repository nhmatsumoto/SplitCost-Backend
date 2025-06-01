using MapsterMapper;
using SplitCost.Application.Common;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases.GetExpense;

public class GetExpensesByResidenceIdUseCase : IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>>
{
    private readonly IMapper _mapper;
    private readonly IExpenseRepository _expenseRepository;
    public GetExpensesByResidenceIdUseCase(
        IMapper mapper,
        IExpenseRepository expenseRepository)
    {
        _mapper             = mapper            ?? throw new ArgumentNullException(nameof(mapper));
        _expenseRepository  = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
    }
    public async Task<Result<IEnumerable<GetExpenseByIdOutput>>> ExecuteAsync(Guid input)
    {
        var expense = await _expenseRepository.GetByResidenceIdAsync(input);

        if (expense == null || !expense.Any())
        {
            return Result<IEnumerable<GetExpenseByIdOutput>>.Failure($"No expenses found for residence ID '{input}'.", ErrorType.NotFound);
        }

        var output = _mapper.Map<IEnumerable<GetExpenseByIdOutput>>(expense);

        return Result<IEnumerable<GetExpenseByIdOutput>>.Success(output);
    }
}
