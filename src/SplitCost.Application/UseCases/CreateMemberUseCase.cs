using FluentValidation;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos.AppMember;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases;

public class CreateMemberUseCase : BaseUseCase<AddMemberInput, Member>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IValidator<AddMemberInput> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateMemberUseCase> _logger;

    public CreateMemberUseCase(
        IMemberRepository memberRepository,
        IValidator<AddMemberInput> validator,
        IUnitOfWork unitOfWork,
        ILogger<CreateMemberUseCase> logger)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<FluentValidation.Results.ValidationResult> ValidateAsync(AddMemberInput input, CancellationToken cancellationToken)
    {
        if (input == null)
        {
            return new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input), "Input não pode ser nulo") }
            );
        }

        return await _validator.ValidateAsync(input, cancellationToken);
    }

    protected override async Task<Result<Member>> HandleAsync(AddMemberInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("Adicionando membro à residência {ResidenceId}", input.ResidenceId);

        var member = MemberFactory.Create()
            .SetUserId(input.UserId)
            .SetResidenceId(input.ResidenceId)
            .SetJoinedAt(DateTime.UtcNow);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _memberRepository.AddAsync(member, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<Member>.Success(member);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Erro ao adicionar membro à residência {ResidenceId}", input.ResidenceId);
            return Result<Member>.Failure(Messages.ResidenceCreationFailed, ErrorType.InternalError);
        }
    }
}
