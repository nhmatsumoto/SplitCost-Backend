using FluentValidation;
using SplitCost.Application.DTOs;

namespace SplitCost.Application.Validators;

public class CreateExpenseDtoValidator : AbstractValidator<CreateExpenseDto>
{
    public CreateExpenseDtoValidator()
    {
        RuleFor(x => x.Type).IsInEnum().WithMessage("Invalid Expense Type.");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.");
        RuleFor(x => x.ResidenceId).NotEmpty().WithMessage("Residence is required.");
        RuleFor(x => x.RegisterByUserId).NotEmpty().WithMessage("RegisterByUser is required.");
        RuleFor(x => x.PaidByUserId).NotEmpty().WithMessage("PaidByUser is required.");
    }
}
