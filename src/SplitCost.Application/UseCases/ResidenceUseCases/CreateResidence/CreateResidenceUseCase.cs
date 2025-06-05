
using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases.ResidenceUseCases.CreateResidence;

public class CreateResidenceUseCase : IUseCase<CreateResidenceInput, Result<CreateResidenceOutput>>
{
    private readonly IResidenceRepository               _residenceRepository;
    private readonly IMemberRepository                  _memberRepository;
    private readonly IUnitOfWork                        _unitOfWork;
    private readonly IMapper                            _mapper;
    private readonly IValidator<CreateResidenceInput>   _validator;
    private readonly ILogger<CreateResidenceUseCase>    _logger;

    public CreateResidenceUseCase(
        IResidenceRepository residenceRepository,
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateResidenceInput> validator,
        ILogger<CreateResidenceUseCase> logger)
    {
        _residenceRepository    = residenceRepository   ?? throw new ArgumentNullException(nameof(residenceRepository));
        _memberRepository       = memberRepository      ?? throw new ArgumentNullException(nameof(memberRepository));
        _unitOfWork             = unitOfWork            ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper                 = mapper                ?? throw new ArgumentNullException(nameof(mapper));
        _validator              = validator             ?? throw new ArgumentNullException(nameof(validator));
        _logger                 = logger                ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<CreateResidenceOutput>> ExecuteAsync(CreateResidenceInput input, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(input);

        if (!validationResult.IsValid)
        {
            return Result<CreateResidenceOutput>.FromFluentValidation(Messages.InvalidData, validationResult.Errors);
        }

        var residence = ResidenceFactory
            .Create()
            .SetName(input.ResidenceName)
            .SetCreatedByUser(input.UserId)
            .SetStreet(input.Street)
            .SetNumber(input.Number)
            .SetApartment(input.Apartment)
            .SetCity(input.City)
            .SetPrefecture(input.Prefecture)
            .SetCountry(input.Country)
            .SetPostalCode(input.PostalCode);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var createdResidence = await _residenceRepository.AddAsync(residence, cancellationToken);

            var member = MemberFactory
                .Create()
                .SetUserId(input.UserId)
                .SetResidenceId(createdResidence.Id)
                .SetJoinedAt(DateTime.UtcNow);
            
            var createdMember = await _memberRepository.AddAsync(member, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            createdResidence.AddMember(createdMember);

            _logger.LogInformation("Residência e membro criados com sucesso: {ResidenceId}, {MemberId}", createdResidence.Id, createdMember.Id);

            return Result<CreateResidenceOutput>.Success(_mapper.Map<CreateResidenceOutput>(createdResidence));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Erro ao persistir residência e membro");
            return Result<CreateResidenceOutput>.Failure(Messages.ResidenceCreationFailed, ErrorType.InternalError);
        }
    }
}
