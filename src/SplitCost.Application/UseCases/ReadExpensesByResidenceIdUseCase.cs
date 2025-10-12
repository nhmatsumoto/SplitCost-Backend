using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos.AppExpense;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class ReadExpensesByResidenceIdUseCase : BaseUseCase<Guid, IEnumerable<GetExpenseByIdOutput>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ILogger<ReadExpensesByResidenceIdUseCase> _logger;

    public ReadExpensesByResidenceIdUseCase(
        IExpenseRepository expenseRepository,
        ILogger<ReadExpensesByResidenceIdUseCase> logger)
    {
        _expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override Task<FluentValidation.Results.ValidationResult> ValidateAsync(Guid residenceId, CancellationToken cancellationToken)
    {
        if (residenceId == Guid.Empty)
        {
            return Task.FromResult(new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(residenceId), "ResidenceId não pode ser vazio") }
            ));
        }

        return Task.FromResult(new FluentValidation.Results.ValidationResult());
    }

    protected override async Task<Result<IEnumerable<GetExpenseByIdOutput>>> HandleAsync(Guid residenceId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("Buscando despesas para residência {ResidenceId}", residenceId);

        var expenses = await _expenseRepository.GetExpenseListByResidenceId(residenceId, cancellationToken);

        if (expenses == null || !expenses.Any())
        {
            _logger.LogWarning("Nenhuma despesa encontrada para residência {ResidenceId}", residenceId);
            return Result<IEnumerable<GetExpenseByIdOutput>>.Failure(
                $"No expenses found for residence ID '{residenceId}'.",
                ErrorType.NotFound
            );
        }

        var output = Mapper.MapList<Expense, GetExpenseByIdOutput>(expenses);

        return Result<IEnumerable<GetExpenseByIdOutput>>.Success(output);
    }
}
