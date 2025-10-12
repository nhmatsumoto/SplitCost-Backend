using FluentValidation;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Dtos.AppResidence;

namespace SplitCost.Application.Validations;

public class CreateResidenceInputValidator : AbstractValidator<CreateResidenceInput>
{
    private readonly IResidenceRepository _residenceRepository;

    public CreateResidenceInputValidator(
        IResidenceRepository residenceRepository)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));

        RuleFor(x => x.ResidenceName)
            .NotEmpty().WithMessage("O nome da residência é obrigatório.")
            .MaximumLength(100).WithMessage("O nome da residência deve ter no máximo 100 caracteres.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("Usuário inválido.");

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

        //RuleFor(x => x.UserId)
        //    .NotNull()
        //    .MustAsync(UserHasResidence).WithMessage("Usuário já possui uma residência.");
    }

}
