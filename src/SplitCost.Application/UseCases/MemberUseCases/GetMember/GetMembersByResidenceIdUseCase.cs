using FluentValidation;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;

namespace SplitCost.Application.UseCases.MemberUseCases.GetMember;

public class GetMembersByResidenceIdUseCase : IUseCase<GetMemberByResidenceIdInput, Result<Dictionary<Guid, string>>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IValidator<GetMemberByResidenceIdInput> _validator;
    public GetMembersByResidenceIdUseCase(IMemberRepository memberRepository, IValidator<GetMemberByResidenceIdInput> validator)
    {
        _memberRepository   = memberRepository  ?? throw new ArgumentException(nameof(memberRepository));
        _validator          = validator         ?? throw new ArgumentException(nameof(validator));
    }

    //Obtem todos os membros de uma residência?
    public async Task<Result<Dictionary<Guid, string>>> ExecuteAsync(GetMemberByResidenceIdInput input, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Result<Dictionary<Guid, string>>.FromFluentValidation(Messages.InvalidData,  validationResult.Errors);
        }

        var members = await _memberRepository.GetUsersByResidenceId(input.ResidenceId, cancellationToken);

        if (members is null || !members.Any())
            return Result<Dictionary<Guid, string>>.Failure("No member found for this residence", ErrorType.NotFound);

        return Result<Dictionary<Guid, string>>.Success(members);
    }
}

