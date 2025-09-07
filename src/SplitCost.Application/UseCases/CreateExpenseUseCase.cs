using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Dtos;
using SplitCost.Application.Mappers;

namespace SplitCost.Application.UseCases;

public class CreateExpenseUseCase : IUseCase<CreateExpenseInput, Result<CreateExpenseOutput>>
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
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<CreateExpenseOutput>> ExecuteAsync(CreateExpenseInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var validationResult = await _validator.ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.BeginScope("Falha de validação ao criar despesa para residência {ResidenceId}", input.PaidByUserId);
            return Result<CreateExpenseOutput>.FromFluentValidation("Dados inválidos", validationResult.Errors);
        }

        var expense = ExpenseMapper.ToDomain(input);

        await _expenseRepository.AddAsync(expense, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        //Revisar o compartilhamento de despesas entre membros
        var result = ExpenseMapper.ToOutput(expense, input.IsSharedAmongMembers);

        return Result<CreateExpenseOutput>.Success(result);
    }
}
