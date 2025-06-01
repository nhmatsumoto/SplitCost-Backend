using FluentValidation;
using SplitCost.Application.UseCases.CreateMember;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.Validators;

public class AddResidenceMemberInputValidator : AbstractValidator<AddResidenceMemberInput>
{
    private readonly IUserRepository _userRepository;
    private readonly IResidenceRepository _residenceRepository;

    public AddResidenceMemberInputValidator(IUserRepository userRepository, IResidenceRepository residenceRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));

        RuleFor(x => x.ResidenceId)
            .NotEmpty()
            .WithMessage("Residence is required.")
            .MustAsync(ResidenceExists)
            .WithMessage("Residence not found.");

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

    private async Task<bool> ResidenceExists(Guid residenceId, CancellationToken ct)
    {
        return await _residenceRepository.ExistsAsync(residenceId, ct);
    }
}
