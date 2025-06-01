using FluentValidation;
using SplitCost.Application.UseCases.CreateResidence;

namespace SplitCost.Application.Validators;

public class CreateAddressInputValidator : AbstractValidator<CreateAddressInput>
{
    public CreateAddressInputValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("A rua é obrigatória.")
            .MaximumLength(150);

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("O número é obrigatório.")
            .MaximumLength(10);

        RuleFor(x => x.Apartment)
            .MaximumLength(10).When(x => !string.IsNullOrWhiteSpace(x.Apartment));

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("A cidade é obrigatória.")
            .MaximumLength(100);

        RuleFor(x => x.Prefecture)
            .NotEmpty().WithMessage("A província é obrigatória.")
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("O país é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("O CEP é obrigatório.")
            .Matches(@"^\d{3}-\d{4}$").WithMessage("CEP inválido. Formato esperado: 123-4567");
    }
}
