using FluentValidation;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.UseCases.GetResidence;

namespace SplitCost.Application.Validators;

public class GetResidenceByUserIdInputValidator : AbstractValidator<GetResidenceByUserIdInput>
{

    private readonly IUserRepository _userRepository;
    public GetResidenceByUserIdInputValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        RuleFor(x => x.UserId)
           .NotEmpty()
           .WithMessage("User is required.")
           .MustAsync(UserExists)
           .WithMessage("User not found.");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken ct)
    {
        return await _userRepository.ExistsAsync(userId, ct);
    }
}
