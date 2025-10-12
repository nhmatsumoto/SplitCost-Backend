using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos.AppExpense;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class ReadExpenseByIdUseCase : BaseUseCase<Guid, GetExpenseByIdOutput>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ILogger<ReadExpenseByIdUseCase> _logger;

    public ReadExpenseByIdUseCase(
        IExpenseRepository expenseRepository,
        ILogger<ReadExpenseByIdUseCase> logger)
    {
        _expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override Task<FluentValidation.Results.ValidationResult> ValidateAsync(Guid input, CancellationToken cancellationToken)
    {
        if (input == Guid.Empty)
        {
            return Task.FromResult(new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input), "Id não pode ser vazio") }
            ));
        }

        return Task.FromResult(new FluentValidation.Results.ValidationResult());
    }

    protected override async Task<Result<GetExpenseByIdOutput>> HandleAsync(Guid input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("Buscando despesa com Id {ExpenseId}", input);

        var expense = await _expenseRepository.GetByIdAsync(input, cancellationToken);

        if (expense == null)
        {
            _logger.LogWarning("Despesa não encontrada: {ExpenseId}", input);
            return Result<GetExpenseByIdOutput>.Failure("Expense not found.", ErrorType.NotFound);
        }

        var expenseDto = Mapper.Map<Expense, GetExpenseByIdOutput>(expense);

        return Result<GetExpenseByIdOutput>.Success(expenseDto);
    }
}
