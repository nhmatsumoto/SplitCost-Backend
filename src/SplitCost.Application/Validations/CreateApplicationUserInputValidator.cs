using FluentValidation;
using SplitCost.Application.Dtos.AppUser;

namespace SplitCost.Application.Validations;

public class CreateApplicationUserInputValidator : AbstractValidator<CreateApplicationUserInput>
{
    public CreateApplicationUserInputValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Nome de usuário é obrigatório.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirme a senha.")
            .Equal(x => x.Password).WithMessage("As senhas não coincidem.");

        RuleFor(x => x.Profile)
            .IsInEnum().WithMessage("Selecione um perfil válido.");
    }
}
