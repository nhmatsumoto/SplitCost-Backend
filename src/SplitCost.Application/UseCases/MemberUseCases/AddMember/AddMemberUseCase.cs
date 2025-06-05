using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases.MemberUseCases.AddMember;

public class AddMemberUseCase : IUseCase<AddMemberInput, Result<AddMemberOutput>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IValidator<AddMemberInput> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AddMemberUseCase> _logger;

    public AddMemberUseCase(
        IUserRepository userRepository,
        IMemberRepository memberRepository, 
        IValidator<AddMemberInput> validator,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AddMemberUseCase> logger)
    {
        _memberRepository   = memberRepository  ?? throw new ArgumentNullException(nameof(memberRepository));
        _validator          = validator         ?? throw new ArgumentNullException(nameof(validator));
        _unitOfWork         = unitOfWork        ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper             = mapper            ?? throw new ArgumentNullException(nameof(mapper));
        _logger             = logger            ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<AddMemberOutput>> ExecuteAsync(AddMemberInput memberInput, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(memberInput);

        if(!validationResult.IsValid)
        {
            return Result<AddMemberOutput>.FromFluentValidation(Messages.InvalidData, validationResult.Errors);
        }

        var member = MemberFactory.Create()
            .SetUserId(memberInput.UserId)
            .SetResidenceId(memberInput.ResidenceId)
            .SetJoinedAt(DateTime.UtcNow);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var createdMember = await _memberRepository.AddAsync(member, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result<AddMemberOutput>.Success(_mapper.Map<AddMemberOutput>(createdMember));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Erro ao adicionar membro à residência");
            return Result<AddMemberOutput>.Failure(Messages.ResidenceCreationFailed, ErrorType.InternalError);
        }
    }
}