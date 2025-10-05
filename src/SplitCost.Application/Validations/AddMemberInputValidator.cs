using FluentValidation;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Dtos;

namespace SplitCost.Application.Validations;

public class AddMemberInputValidator : AbstractValidator<AddMemberInput>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IResidenceRepository _residenceRepository;

    public AddMemberInputValidator(
        IResidenceRepository residenceRepository,
        IMemberRepository memberRepository)
    {
        _residenceRepository    = residenceRepository   ?? throw new ArgumentNullException(nameof(residenceRepository));
        _memberRepository       = memberRepository      ?? throw new ArgumentNullException(nameof(memberRepository));

        //RuleFor(x => x.UserId)
        //    .NotEmpty().WithMessage("User is required.")
        //    .MustAsync(UserExists).WithMessage("Usuário não encontrado.");

        RuleFor(x => x.ResidenceId)
            .NotEmpty().WithMessage("Residence is required.")
            .MustAsync(ResidenceExists).WithMessage("Residência não encontrada.");

        RuleFor(x => x)
            .MustAsync(NotAlreadyMember).WithMessage("Usuário já é membro dessa residência.");
    }

    private async Task<bool> ResidenceExists(Guid residenceId, CancellationToken cancellationToken)
    {
        return await _residenceRepository.ExistsAsync(x => x.Id == residenceId, cancellationToken);
    }

    private async Task<bool> NotAlreadyMember(AddMemberInput input, CancellationToken cancellationToken)
    {
        var exists = await _memberRepository.ExistsAsync(x => x.UserId == input.UserId && x.ResidenceId == input.ResidenceId, cancellationToken);
        return !exists;
    }
}
