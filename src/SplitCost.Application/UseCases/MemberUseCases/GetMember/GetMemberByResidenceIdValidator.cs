using FluentValidation;
using SplitCost.Application.Common.Repositories;

namespace SplitCost.Application.UseCases.MemberUseCases.GetMember;

public class GetMemberByResidenceIdValidator : AbstractValidator<GetMemberByResidenceIdInput>
{
    //Obter membro por Id da residencia
    
    private readonly IMemberRepository _memberRepository;

    public GetMemberByResidenceIdValidator(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        
        RuleFor(x => x.ResidenceId)
            .NotEqual(Guid.Empty).WithMessage("Residence ID cannot be empty.")
            .MustAsync(MemberExists).WithMessage("Este usuário não está em nenhuma residência");
    }

    private async Task<bool> MemberExists(Guid residenceId, CancellationToken cancellationToken)
    {
        return await _memberRepository.ExistsByResidenceId(residenceId, cancellationToken);
    }

}
