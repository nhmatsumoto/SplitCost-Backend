using FluentValidation;
using SplitCost.Application.Dtos;

namespace SplitCost.Application.Validations;

public class GetResidenceByUserIdInputValidator : AbstractValidator<GetResidenceByUserIdInput>
{
    public GetResidenceByUserIdInputValidator()
    {
        RuleFor(x => x.UserId)
           .NotEmpty()
           .WithMessage("User is required.");
    }
}
