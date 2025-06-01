using FluentValidation;
using SplitCost.Application.UseCases.CreateResidence;

namespace SplitCost.Application.Validators;

public class CreateResidenceInputValidator : AbstractValidator<CreateResidenceInput>
{
    public CreateResidenceInputValidator()
    {
        RuleFor(x => x.ResidenceName)
            .NotEmpty().WithMessage("O nome da residência é obrigatório.")
            .MaximumLength(100).WithMessage("O nome da residência deve ter no máximo 100 caracteres.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("Usuário inválido.");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Endereço é obrigatório.")
            .SetValidator(new CreateAddressInputValidator());
    }
}
