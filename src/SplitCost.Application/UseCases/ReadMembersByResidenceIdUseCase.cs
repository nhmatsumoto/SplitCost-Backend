using FluentValidation;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.UseCases.Dtos;

namespace SplitCost.Application.UseCases;

public class ReadMembersByResidenceIdUseCase : IUseCase<GetMemberByResidenceIdInput, Result<Dictionary<Guid, string>>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IValidator<GetMemberByResidenceIdInput> _validator;
    public ReadMembersByResidenceIdUseCase(IMemberRepository memberRepository, IValidator<GetMemberByResidenceIdInput> validator)
    {
        _memberRepository   = memberRepository  ?? throw new ArgumentException(nameof(memberRepository));
        _validator          = validator         ?? throw new ArgumentException(nameof(validator));
    }

    //Obtem todos os membros de uma residência?
    public async Task<Result<Dictionary<Guid, string>>> ExecuteAsync(GetMemberByResidenceIdInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();


        var validationResult = await _validator.ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Result<Dictionary<Guid, string>>.FromFluentValidation(Messages.InvalidData,  validationResult.Errors);
        }

        var query = await _memberRepository.GetByPredicateAsync(x => x.Id == input.ResidenceId, cancellationToken);
        var members = query.ToDictionary(x => x.UserId, x => x.User.Name);

        if (members is null || !members.Any())
            return Result<Dictionary<Guid, string>>.Failure("No member found for this residence", ErrorType.NotFound);

        return Result<Dictionary<Guid, string>>.Success(members);
    }
}

