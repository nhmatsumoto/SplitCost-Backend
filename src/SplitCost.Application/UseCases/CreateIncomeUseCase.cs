using FluentValidation;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos.AppIncome;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases;

public class CreateIncomeUseCase : BaseUseCase<CreateIncomeInput, CreateIncomeOutput>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateIncomeInput> _validator;
    private readonly ILogger<CreateIncomeUseCase> _logger;

    public CreateIncomeUseCase(
        IIncomeRepository incomeRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateIncomeInput> validator,
        ILogger<CreateIncomeUseCase> logger)
    {
        _incomeRepository = incomeRepository ?? throw new ArgumentNullException(nameof(incomeRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<FluentValidation.Results.ValidationResult> ValidateAsync(CreateIncomeInput input, CancellationToken cancellationToken)
    {
        if (input == null)
        {
            return new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input), "Input não pode ser nulo") }
            );
        }

        if (input.Amount <= 0)
        {
            return new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input.Amount), "O valor da renda deve ser maior que zero") }
            );
        }

        return await _validator.ValidateAsync(input, cancellationToken);
    }

    protected override async Task<Result<CreateIncomeOutput>> HandleAsync(CreateIncomeInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("Criando renda para residência {ResidenceId}", input.ResidenceId);

        var income = IncomeFactory
            .Create()
            .SetAmount(input.Amount)
            .SetCategory(input.Category)
            .SetDate(input.Date)
            .SetResidenceId(input.ResidenceId)
            .SetUserId(input.UserId)
            .SetDescription(input.Description);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _incomeRepository.AddAsync(income, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            var result = Mapper.Map<Income, CreateIncomeOutput>(income);
            return Result<CreateIncomeOutput>.Success(result);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Operação cancelada ao criar renda para residência {ResidenceId}", input.ResidenceId);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao criar renda para residência {ResidenceId}", input.ResidenceId);
            await _unitOfWork.RollbackAsync(cancellationToken);
            return Result<CreateIncomeOutput>.Failure("Erro interno ao criar a renda.", ErrorType.InternalError);
        }
    }
}
