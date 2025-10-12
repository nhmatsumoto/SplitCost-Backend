using FluentValidation;
using SplitCost.Application.Dtos.AppResidence;

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
