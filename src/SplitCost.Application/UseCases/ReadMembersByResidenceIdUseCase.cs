using FluentValidation;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class ReadMembersByResidenceIdUseCase : BaseUseCase<GetMemberByResidenceIdInput, IEnumerable<MemberItemDto>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IValidator<GetMemberByResidenceIdInput> _validator;
    private readonly ILogger<ReadMembersByResidenceIdUseCase> _logger;

    public ReadMembersByResidenceIdUseCase(
        IMemberRepository memberRepository,
        IValidator<GetMemberByResidenceIdInput> validator,
        ILogger<ReadMembersByResidenceIdUseCase> logger)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<FluentValidation.Results.ValidationResult> ValidateAsync(GetMemberByResidenceIdInput input, CancellationToken cancellationToken)
    {
        if (input == null)
        {
            return new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input), "Input não pode ser nulo") }
            );
        }

        return await _validator.ValidateAsync(input, cancellationToken);
    }

    protected override async Task<Result<IEnumerable<MemberItemDto>>> HandleAsync(GetMemberByResidenceIdInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("Buscando membros da residência {ResidenceId}", input.ResidenceId);

        var members = await _memberRepository.GetByPredicateAsync(x => x.ResidenceId == input.ResidenceId, cancellationToken);

        var output = Mapper.MapList<Member, MemberItemDto>(members);

        if (output == null || !output.Any())
        {
            _logger.LogWarning("Nenhum membro encontrado para residência {ResidenceId}", input.ResidenceId);
            return Result<IEnumerable<MemberItemDto>>.Failure("No member found for this residence", ErrorType.NotFound);
        }

        return Result<IEnumerable<MemberItemDto>>.Success(output);
    }
}
