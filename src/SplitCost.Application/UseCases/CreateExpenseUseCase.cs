using FluentValidation;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos.AppExpense;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class CreateExpenseUseCase : BaseUseCase<CreateExpenseInput, CreateExpenseOutput>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateExpenseInput> _validator;
    private readonly ILogger<CreateExpenseUseCase> _logger;

    public CreateExpenseUseCase(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateExpenseInput> validator,
        ILogger<CreateExpenseUseCase> logger
    )
    {
        _expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // Validação defensiva antes do HandleAsync
    protected override async Task<FluentValidation.Results.ValidationResult> ValidateAsync(CreateExpenseInput input, CancellationToken cancellationToken)
    {
        if (input == null)
        {
            return new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input), "Input não pode ser nulo") }
            );
        }

        // validação adicional básica
        if (input.Amount <= 0)
        {
            return new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input.Amount), "O valor da despesa deve ser maior que zero") }
            );
        }

        return await _validator.ValidateAsync(input, cancellationToken);
    }

    protected override async Task<Result<CreateExpenseOutput>> HandleAsync(CreateExpenseInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("Criando despesa para residência {ResidenceId}", input.ResidenceId);

        var expense = Mapper.Map<CreateExpenseInput, Expense>(input);

        try
        {
            await _expenseRepository.AddAsync(expense, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            var result = Mapper.Map<Expense, CreateExpenseOutput>(expense);

            return Result<CreateExpenseOutput>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar despesa para residência {ResidenceId}", input.ResidenceId);
            return Result<CreateExpenseOutput>.Failure("Falha ao criar despesa.", ErrorType.InternalError);
        }
    }
}
