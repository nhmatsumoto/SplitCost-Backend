
using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.UseCases.Dtos;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases;

public class CreateResidenceUseCase : IUseCase<CreateResidenceInput, Result<Residence>>
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

    public async Task<Result<Residence>> ExecuteAsync(CreateResidenceInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var validationResult = await _validator.ValidateAsync(input);

        if (!validationResult.IsValid)
        {
            return Result<Residence>.FromFluentValidation(Messages.InvalidData, validationResult.Errors);
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
            await _residenceRepository.AddAsync(residence, cancellationToken);

            var member = MemberFactory
                .Create()
                .SetUserId(input.UserId)
                .SetResidenceId(residence.Id)
                .SetJoinedAt(DateTime.UtcNow);

            await _memberRepository.AddAsync(member, cancellationToken);


            await _unitOfWork.CommitAsync(cancellationToken);

           

            return Result<Residence>.Success(residence);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Erro ao persistir residência e membro");
            return Result<Residence>.Failure(Messages.ResidenceCreationFailed, ErrorType.InternalError);
        }
    }
}
