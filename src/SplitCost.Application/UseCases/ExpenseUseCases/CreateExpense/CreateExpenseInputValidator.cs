using FluentValidation;
using SplitCost.Application.Common.Repositories;

namespace SplitCost.Application.UseCases.ExpenseUseCases.CreateExpense;

public class CreateExpenseInputValidator : AbstractValidator<CreateExpenseInput>
{
    private readonly IUserRepository _userRepository;
    private readonly IResidenceRepository _residenceRepository;

    public CreateExpenseInputValidator(
        IUserRepository userRepository,
        IResidenceRepository residenceRepository)
    {
        _userRepository = userRepository;
        _residenceRepository = residenceRepository;

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid Expense Type.");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("A descrição é obrigatória.");

        RuleFor(x => x.ResidenceId)
            .NotEmpty()
            .WithMessage("Residence is required.")
            .MustAsync(ResidenceExists)
            .WithMessage("Residence not found.");

        RuleFor(x => x.PaidByUserId)
            .NotEmpty()
            .WithMessage("PaidByUser is required.")
            .MustAsync(UserExists)
            .WithMessage("Paying user not found.");

        RuleFor(x => x.RegisteredByUserId)
            .NotEmpty()
            .WithMessage("RegisterByUser is required.")
            .MustAsync(UserExists)
            .WithMessage("Registered user not found.");
    }

    private async Task<bool> ResidenceExists(Guid residenceId, CancellationToken ct)
    {
        return await _residenceRepository.ExistsAsync(residenceId, ct);
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken ct)
    {
        return await _userRepository.ExistsAsync(userId, ct);
    }
}