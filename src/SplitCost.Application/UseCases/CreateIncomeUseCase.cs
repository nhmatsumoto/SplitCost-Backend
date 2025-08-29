using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.UseCases.Dtos;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases;

public class CreateIncomeUseCase(
    IIncomeRepository incomeRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateIncomeInput> validator,
    ILogger<CreateIncomeUseCase> logger) : IUseCase<CreateIncomeInput, Result<CreateIncomeOutput>>
{
    private readonly IIncomeRepository                  _incomeRepository = incomeRepository    ?? throw new ArgumentNullException(nameof(incomeRepository));
    private readonly IUnitOfWork                        _unitOfWork = unitOfWork                ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper                            _mapper = mapper                        ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IValidator<CreateIncomeInput>      _validator = validator                  ?? throw new ArgumentNullException(nameof(validator));
    private readonly ILogger<CreateIncomeUseCase>       _logger = logger                        ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result<CreateIncomeOutput>> ExecuteAsync(CreateIncomeInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var validationResult = await _validator.ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Falha de validação ao criar renda para residência {ResidenceId}: {Errors}",
               input.ResidenceId, validationResult.Errors);

            return Result<CreateIncomeOutput>.FromFluentValidation(
                $"Falha ao criar renda para residência {input.ResidenceId}",
                validationResult.Errors);
        }

        var transactionStarted = false;

        try
        {
            var income = IncomeFactory
                 .Create()
                 .SetAmount(input.Amount)
                 .SetCategory(input.Category)
                 .SetDate(input.Date)
                 .SetResidenceId(input.ResidenceId)
                 .SetUserId(input.UserId)
                 .SetDescription(input.Description);

            
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            transactionStarted = true;

            await _incomeRepository.AddAsync(income, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            var result = _mapper.Map<CreateIncomeOutput>(income);
            return Result<CreateIncomeOutput>.Success(result);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Operação cancelada ao criar renda para residência {ResidenceId}", input.ResidenceId);
            if (transactionStarted) await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao criar renda para residência {ResidenceId}", input.ResidenceId);
            if (transactionStarted) await _unitOfWork.RollbackAsync(cancellationToken);
            return Result<CreateIncomeOutput>.Failure(
                "Erro interno ao criar a renda.",
                ErrorType.InternalError
            );
        }

    }
}
