using FluentValidation;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Dtos;

namespace SplitCost.Application.Validations;

public class CreateExpenseInputValidator : AbstractValidator<CreateExpenseInput>
{
    private readonly IResidenceRepository _residenceRepository;

    public CreateExpenseInputValidator(
        IResidenceRepository residenceRepository)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid Expense Type.");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");

        //RuleFor(x => x.Date)
        //    .NotEmpty()
        //    .WithMessage("Date is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("A descrição é obrigatória.");

        //RuleFor(x => x.ResidenceId)
        //    .NotEmpty()
        //    .WithMessage("Residence is required.")
        //    .MustAsync(ResidenceExists)
        //    .WithMessage("Residence not found.");

        //RuleFor(x => x.PaidByUserId)
        //    .NotEmpty()
        //    .WithMessage("PaidByUser is required.")
        //    .MustAsync(UserExists)
        //    .WithMessage("Paying user not found.");

        //RuleFor(x => x.RegisteredByUserId)
        //    .NotEmpty()
        //    .WithMessage("RegisterByUser is required.")
        //    .MustAsync(UserExists)
        //    .WithMessage("Registered user not found.");
    }

    private async Task<bool> ResidenceExists(Guid residenceId, CancellationToken ct)
    {
        return await _residenceRepository.ExistsAsync(x => x.Id == residenceId, ct);
    }

}