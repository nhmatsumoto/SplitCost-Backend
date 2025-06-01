using SplitCost.Application.Common;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases.GetMember;

public class GetMemberByResidenceIdUseCase : IUseCase<Guid, Result<Dictionary<Guid, string>>>
{
    private readonly IMemberRepository _memberRepository;
    public GetMemberByResidenceIdUseCase(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository ?? throw new ArgumentException(nameof(memberRepository));
    }

    public async Task<Result<Dictionary<Guid, string>>> ExecuteAsync(Guid residenceId)
    {
        if (residenceId == Guid.Empty)
            return Result<Dictionary<Guid, string>>.Failure("Invalid Residence", ErrorType.Validation);

        var members = await _memberRepository.GetUsersByResidenceId(residenceId);

        if (members is null || !members.Any())
            return Result<Dictionary<Guid, string>>.Failure("No member found for this residence", ErrorType.NotFound);

        return Result<Dictionary<Guid, string>>.Success(members);
    }
}

