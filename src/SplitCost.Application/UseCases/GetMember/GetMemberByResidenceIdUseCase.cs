using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;

namespace SplitCost.Application.UseCases.GetMember;

public class GetMemberByResidenceIdUseCase : IUseCase<Guid, Result<Dictionary<Guid, string>>>
{
    private readonly IMemberRepository _memberRepository;
    public GetMemberByResidenceIdUseCase(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository ?? throw new ArgumentException(nameof(memberRepository));
    }

    public async Task<Result<Dictionary<Guid, string>>> ExecuteAsync(Guid residenceId, CancellationToken cancellationToken)
    {
        if (residenceId == Guid.Empty)
            return Result<Dictionary<Guid, string>>.Failure("Invalid Residence", ErrorType.Validation);

        var members = await _memberRepository.GetUsersByResidenceId(residenceId, cancellationToken);

        if (members is null || !members.Any())
            return Result<Dictionary<Guid, string>>.Failure("No member found for this residence", ErrorType.NotFound);

        return Result<Dictionary<Guid, string>>.Success(members);
    }
}

