using MapsterMapper;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;

namespace SplitCost.Application.UseCases.GetExpense;

public class GetExpenseByIdUseCase : IUseCase<Guid, Result<GetExpenseByIdOutput>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public GetExpenseByIdUseCase(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository ?? throw new ArgumentException(nameof(expenseRepository));
        _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
    }

    public async Task<Result<GetExpenseByIdOutput>> ExecuteAsync(Guid input, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(input, cancellationToken);

        if (expense == null)
        {
            return Result<GetExpenseByIdOutput>.Failure($"Expense not found.", ErrorType.NotFound);
        }

        var expenseDto = _mapper.Map<GetExpenseByIdOutput>(expense);

        return Result<GetExpenseByIdOutput>.Success(expenseDto);
    }
}
