using FluentValidation;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.UseCases.CreateResidence;

namespace SplitCost.Application.Validators;

public class CreateResidenceInputValidator : AbstractValidator<CreateResidenceInput>
{

    private readonly IResidenceRepository _residenceRepository;


    // Verificar se usuário já tem uma rediência
    public CreateResidenceInputValidator(
        IResidenceRepository residenceRepository)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));

        RuleFor(x => x.ResidenceName)
            .NotEmpty().WithMessage("O nome da residência é obrigatório.")
            .MaximumLength(100).WithMessage("O nome da residência deve ter no máximo 100 caracteres.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("Usuário inválido.");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Endereço é obrigatório.")
            .SetValidator(new CreateAddressInputValidator());

        RuleFor(x => x.UserId)
            .NotNull()
            .MustAsync(UserHasResidence).WithMessage("Usuário já possui uma residência.");
    }

    private async Task<bool> UserHasResidence(Guid userId, CancellationToken ct)
        => !await _residenceRepository.UserHasResidence(userId, ct);
}
