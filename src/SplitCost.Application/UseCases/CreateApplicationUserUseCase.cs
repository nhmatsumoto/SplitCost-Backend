using FluentValidation;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.Services;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos;
using SplitCost.Domain.Builders;

namespace SplitCost.Application.UseCases;

public class CreateApplicationUserUseCase : BaseUseCase<CreateUserInput, CreateUserOutput>
{
    private readonly IUserRepository _userRepository;
    private readonly IKeycloakService _keycloakService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateUserInput> _validator;
    private readonly ILogger<CreateApplicationUserUseCase> _logger;
    private readonly IUserSettingsRepository _userSettingsRepository;

    public CreateApplicationUserUseCase(
        IUserRepository userRepository,
        IKeycloakService keycloakService,
        IUnitOfWork unitOfWork,
        IValidator<CreateUserInput> validator,
        ILogger<CreateApplicationUserUseCase> logger,
        IUserSettingsRepository userSettingsRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _keycloakService = keycloakService ?? throw new ArgumentNullException(nameof(keycloakService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userSettingsRepository = userSettingsRepository ?? throw new ArgumentNullException(nameof(userSettingsRepository));
    }

    protected override async Task<FluentValidation.Results.ValidationResult> ValidateAsync(CreateUserInput input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.Username))
            return new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input.Username), "Username não pode ser vazio") }
            );

        return await _validator.ValidateAsync(input, cancellationToken);
    }

    protected override async Task<Result<CreateUserOutput>> HandleAsync(CreateUserInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Guid keycloakUserId;
        try
        {
            keycloakUserId = await _keycloakService.CreateUserAsync(
                input.Username,
                input.FirstName,
                input.LastName,
                input.Email,
                input.Password,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário no Keycloak: {Username}", input.Username);
            return Result<CreateUserOutput>.Failure("Erro ao criar usuário no Keycloak.", ErrorType.InternalError);
        }

        if (keycloakUserId == Guid.Empty)
            return Result<CreateUserOutput>.Failure("Erro ao criar usuário no Keycloak.", ErrorType.InternalError);

        var user = new UserBuilder()
            .WithId(keycloakUserId)
            .WithUsername(input.Username)
            .WithName($"{input.FirstName} {input.LastName}")
            .WithEmail(input.Email)
            .WithAvatar(string.Empty)
            .Build();

        var userSettings = new UserSettingsBuilder()
            .WithUserId(user.Id)
            .WithTheme("light")
            .WithLanguage("pt-BR")
            .Build();

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _userRepository.AddAsync(user, cancellationToken);
            await _userSettingsRepository.AddAsync(userSettings, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<CreateUserOutput>.Success(new CreateUserOutput { Id = user.Id });
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Erro ao persistir usuário: {Email}", user.Email);

            try
            {
                await _keycloakService.DeleteUserAsync(keycloakUserId, cancellationToken);
            }
            catch (Exception kcEx)
            {
                _logger.LogError(kcEx, "Falha ao remover usuário do Keycloak após erro: {Username}", user.Username);
            }

            return Result<CreateUserOutput>.Failure("Falha ao criar usuário.", ErrorType.InternalError);
        }
    }
}
