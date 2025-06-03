using FluentValidation;
using MapsterMapper;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases.ExpenseUseCases.CreateExpense;

public class CreateExpenseUseCase : IUseCase<CreateExpenseInput, Result<CreateExpenseOutput>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateExpenseInput> _validator;

    public CreateExpenseUseCase(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateExpenseInput> validator)
    {
        _expenseRepository      = expenseRepository     ?? throw new ArgumentNullException(nameof(expenseRepository));
        _unitOfWork             = unitOfWork            ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper                 = mapper                ?? throw new ArgumentNullException(nameof(mapper));
        _validator              = validator             ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<Result<CreateExpenseOutput>> ExecuteAsync(CreateExpenseInput input, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Result<CreateExpenseOutput>.FromFluentValidation("Dados inválidos", validationResult.Errors);
        }

        //Map DTO To Domain Entity
        var expense = _mapper.Map<Expense>(input);

        // Adiciona ao repositório e salva as mudanças
        await _expenseRepository.AddAsync(expense, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<CreateExpenseOutput>(expense);

        return Result<CreateExpenseOutput>.Success(result);
    }

}
