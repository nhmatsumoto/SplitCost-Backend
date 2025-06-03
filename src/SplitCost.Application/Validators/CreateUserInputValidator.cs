using FluentValidation;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.UseCases.CreateApplicationUser;

namespace SplitCost.Application.Validators;

public class CreateUserInputValidator : AbstractValidator<CreateApplicationUserInput>
{
    private readonly IUserRepository _userRepository;

    public CreateUserInputValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        // Todo : Add validation for unique username
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters long.")
            .MustAsync(BeUniqueUsername).WithMessage("Username is already in use.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(1, 50).WithMessage("First name must be between 1 and 50 characters long.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(1, 50).WithMessage("Last name must be between 1 and 50 characters long.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(BeUniqueEmail).WithMessage("Email is already in use.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match.");
    }

    private async Task<bool> BeUniqueUsername(string username, CancellationToken ct)
    {
        return !await _userRepository.ExistsByUsernameAsync(username, ct);
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken ct)
    {
        return !await _userRepository.ExistsByEmailAsync(email, ct);
    }
}
