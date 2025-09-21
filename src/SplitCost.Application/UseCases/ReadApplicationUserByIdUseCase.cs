using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class GetApplicationUserByIdUseCase : BaseUseCase<Guid, User>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetApplicationUserByIdUseCase> _logger;

    public GetApplicationUserByIdUseCase(
        IUserRepository userRepository,
        ILogger<GetApplicationUserByIdUseCase> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override Task<FluentValidation.Results.ValidationResult> ValidateAsync(Guid input, CancellationToken cancellationToken)
    {
        if (input == Guid.Empty)
        {
            return Task.FromResult(new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input), "Id não pode ser vazio") }
            ));
        }

        return Task.FromResult(new FluentValidation.Results.ValidationResult());
    }

    protected override async Task<Result<User>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("Buscando usuário com Id {UserId}", id);

        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("Usuário não encontrado: {UserId}", id);
            return Result<User>.Failure(Messages.UserNotFound, ErrorType.NotFound);
        }

        return Result<User>.Success(user);
    }
}
