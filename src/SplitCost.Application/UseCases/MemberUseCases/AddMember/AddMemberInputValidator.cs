﻿using FluentValidation;
using SplitCost.Application.Common.Repositories;

namespace SplitCost.Application.UseCases.MemberUseCases.AddMember;

public class AddMemberInputValidator : AbstractValidator<AddMemberInput>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IResidenceRepository _residenceRepository;
    private readonly IUserRepository _userRepository;

    public AddMemberInputValidator(
        IUserRepository userRepository,
        IResidenceRepository residenceRepository,
        IMemberRepository memberRepository)
    {
        _userRepository         = userRepository        ?? throw new ArgumentNullException(nameof(userRepository));
        _residenceRepository    = residenceRepository   ?? throw new ArgumentNullException(nameof(residenceRepository));
        _memberRepository       = memberRepository      ?? throw new ArgumentNullException(nameof(memberRepository));

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User is required.")
            .MustAsync(UserExists).WithMessage("Usuário não encontrado.");

        RuleFor(x => x.ResidenceId)
            .NotEmpty().WithMessage("Residence is required.")
            .MustAsync(ResidenceExists).WithMessage("Residência não encontrada.");

        RuleFor(x => x)
            .MustAsync(NotAlreadyMember).WithMessage("Usuário já é membro dessa residência.");
    }

    private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
    {
        return await _userRepository.ExistsAsync(userId, cancellationToken);
    }

    private async Task<bool> ResidenceExists(Guid residenceId, CancellationToken cancellationToken)
    {
        return await _residenceRepository.ExistsAsync(residenceId, cancellationToken);
    }

    private async Task<bool> NotAlreadyMember(AddMemberInput input, CancellationToken cancellationToken)
    {
        var exists = await _memberRepository.ExistsAsync(input.UserId, input.ResidenceId, cancellationToken);
        return !exists;
    }
}
