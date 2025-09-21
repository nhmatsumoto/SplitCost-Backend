using Microsoft.Extensions.Logging;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class ReadUserSettingsByUserIdUseCase : BaseUseCase<Guid, UserSettings>
{
    private readonly IUserSettingsRepository _userSettingsRepository;
    private readonly ILogger<ReadUserSettingsByUserIdUseCase> _logger;

    public ReadUserSettingsByUserIdUseCase(
        IUserSettingsRepository userSettingsRepository,
        ILogger<ReadUserSettingsByUserIdUseCase> logger)
    {
        _userSettingsRepository = userSettingsRepository ?? throw new ArgumentNullException(nameof(userSettingsRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override Task<FluentValidation.Results.ValidationResult> ValidateAsync(Guid userId, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            return Task.FromResult(new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(userId), "UserId não pode ser vazio") }
            ));
        }

        return Task.FromResult(new FluentValidation.Results.ValidationResult());
    }

    protected override async Task<Result<UserSettings>> HandleAsync(Guid userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("Buscando configurações do usuário {UserId}", userId);

        var userSettings = await _userSettingsRepository.GetByExpression(x => x.UserId == userId, cancellationToken);

        if (userSettings == null)
        {
            _logger.LogWarning("User settings não encontradas para usuário {UserId}", userId);
            return Result<UserSettings>.Failure("User settings not found", ErrorType.NotFound);
        }

        return Result<UserSettings>.Success(userSettings);
    }
}
