using Microsoft.Extensions.Logging;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class ReadUserSettingsByUserIdUseCase : IUseCase<Guid, Result<UserSettings>>
{
    private readonly IUserSettingsRepository _userSettingsRepository;
    private readonly ILogger<ReadUserSettingsByUserIdUseCase> _logger;

    public ReadUserSettingsByUserIdUseCase(IUserSettingsRepository userSettingsRepository,
        ILogger<ReadUserSettingsByUserIdUseCase> logger)
    {
        _userSettingsRepository = userSettingsRepository ?? throw new ArgumentNullException(nameof(userSettingsRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<UserSettings>> ExecuteAsync(Guid userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userSettings = await _userSettingsRepository.GetByExpression(x => x.UserId == userId, cancellationToken);

        if (userSettings is null)
        {
            _logger.LogWarning($"User settings not found for user ID '{userId}'.", userId);
            return Result<UserSettings>.Failure($"User settings not found", ErrorType.NotFound);
        }

        return Result<UserSettings>.Success(userSettings);
    }
}
