using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos.AppResidence;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases;

public class CreateResidenceUseCase : BaseUseCase<CreateResidenceInput, Residence>
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateResidenceInput> _validator;
    private readonly IUserSettingsRepository _userSettingsRepository;
    private readonly ILogger<CreateResidenceUseCase> _logger;

    public CreateResidenceUseCase(
        IResidenceRepository residenceRepository,
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateResidenceInput> validator,
        ILogger<CreateResidenceUseCase> logger,
        IUserSettingsRepository userSettingsRepository)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _userSettingsRepository = userSettingsRepository ?? throw new ArgumentNullException(nameof(userSettingsRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<ValidationResult> ValidateAsync(CreateResidenceInput input, CancellationToken cancellationToken)
    {
        if (input == null)
        {
            return new ValidationResult(
                new[] { new ValidationFailure(nameof(input), "Input não pode ser nulo") }
            );
        }

        return await _validator.ValidateAsync(input, cancellationToken);
    }

    protected override async Task<Result<Residence>> HandleAsync(CreateResidenceInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("CREATE RESIDENCE FOR USER: {UserId}", input.UserId);

        var residence = ResidenceFactory
            .Create()
            .SetName(input.ResidenceName)
            .SetStreet(input.Street!)
            .SetNumber(input.Number!)
            .SetApartment(input.Apartment!)
            .SetCity(input.City!)
            .SetPrefecture(input.Prefecture!)
            .SetCountry(input.Country!)
            .SetPostalCode(input.PostalCode!);

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
            _logger.LogError(ex, "Erro ao persistir residência e membro para usuário {UserId}", input.UserId);
            return Result<Residence>.Failure(Messages.ResidenceCreationFailed, ErrorType.InternalError);
        }
    }
}
