
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

public class CreateResidenceUseCase(
    IResidenceRepository residenceRepository,
    IMemberRepository memberRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateResidenceInput> validator,
    ILogger<CreateResidenceUseCase> logger,
    IUserSettingsRepository userSettingsRepository) : IUseCase<CreateResidenceInput, Result<Residence>>
{
    private readonly IResidenceRepository               _residenceRepository        = residenceRepository       ?? throw new ArgumentNullException(nameof(residenceRepository));
    private readonly IMemberRepository                  _memberRepository           = memberRepository          ?? throw new ArgumentNullException(nameof(memberRepository));
    private readonly IUnitOfWork                        _unitOfWork                 = unitOfWork                ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper                            _mapper                     = mapper                    ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IValidator<CreateResidenceInput>   _validator                  = validator                 ?? throw new ArgumentNullException(nameof(validator));
    private readonly ILogger<CreateResidenceUseCase>    _logger                     = logger                    ?? throw new ArgumentNullException(nameof(logger));
    private readonly IUserSettingsRepository            _userSettingsRepository     = userSettingsRepository    ?? throw new ArgumentNullException(nameof(userSettingsRepository));
    

    public async Task<Result<Residence>> ExecuteAsync(CreateResidenceInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var validationResult = await _validator.ValidateAsync(input);

        if (!validationResult.IsValid)
        {
            return Result<Residence>.FromFluentValidation(Messages.InvalidData, validationResult.Errors);
        }

        //Alguns campos podem ser nulos
        var residence = ResidenceFactory
            .Create()
            .SetName(input.ResidenceName)
            .SetStreet(input.Street)
            .SetNumber(input.Number)
            .SetApartment(input.Apartment)
            .SetCity(input.City)
            .SetPrefecture(input.Prefecture)
            .SetCountry(input.Country)
            .SetPostalCode(input.PostalCode);

        var userSettings = await _userSettingsRepository.GetByExpression(x => x.UserId == input.UserId, cancellationToken);

        if (userSettings is null)
        {
            _logger.LogWarning("User settings not found for user {UserId}", input.UserId);
            return Result<Residence>.Failure("User settings not found", ErrorType.NotFound);
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _residenceRepository.AddAsync(residence, cancellationToken);

            userSettings.SetResidenceId(residence.Id);
            await _userSettingsRepository.UpdateAsync(userSettings, cancellationToken);

            var member = MemberFactory
                .Create()
                .SetId(Guid.NewGuid())
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
