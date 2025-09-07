using FluentValidation;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.Services;
using SplitCost.Application.Dtos;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases;

public class CreateApplicationUserUseCase(
    IUserRepository userRepository,
    IKeycloakService keycloakService,
    IUnitOfWork unitOfWork,
    IValidator<CreateUserInput> validator,
    ILogger<CreateApplicationUserUseCase> logger,
    IUserSettingsRepository userSettingsRepository) : IUseCase<CreateUserInput, Result<CreateUserOutput>>
{
    private readonly IUserRepository                        _userRepository         = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IKeycloakService                       _keycloakService        = keycloakService           ?? throw new ArgumentNullException(nameof(keycloakService));
    private readonly IUnitOfWork                            _unitOfWork             = unitOfWork                ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IValidator<CreateUserInput> _validator              = validator                 ?? throw new ArgumentNullException(nameof(validator));
    private readonly ILogger<CreateApplicationUserUseCase>  _logger                 = logger                    ?? throw new ArgumentNullException(nameof(logger));
    private readonly IUserSettingsRepository                _userSettingsRepository = userSettingsRepository    ?? throw new ArgumentNullException(nameof(userSettingsRepository));

    public async Task<Result<CreateUserOutput>> ExecuteAsync(CreateUserInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var validationResult = await _validator.ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for CreateApplicationUserInput: {Errors}", validationResult.Errors);
            return Result<CreateUserOutput>.FromFluentValidation(
                $"Erro de validação para usuário {input.Username}",
                validationResult.Errors
            );
        }

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
            return Result<CreateUserOutput>.Failure(
                "Erro ao criar usuário no Keycloak. Tente novamente mais tarde.",
                ErrorType.InternalError
            );
        }

        if (keycloakUserId == Guid.Empty)
        {
            return Result<CreateUserOutput>.Failure(
                "Erro ao criar usuário no Keycloak. Verifique os dados informados.",
                ErrorType.InternalError
            );
        }

        var name = $"{input.FirstName} {input.LastName}";

        var user = UserFactory.Create();

        user.SetId(keycloakUserId);
        user.SetUsername(input.Username);
        user.SetAvatarUrl(string.Empty);
        user.SetName(name);
        user.SetEmail(input.Email);

        var userSettings = UserSettingsFactory.Create();

        userSettings.SetUserId(user.Id);
        userSettings.SetTheme("light");
        userSettings.SetLanguage("pt-BR");
        userSettings.SetUserId(user.Id);


        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _userRepository.AddAsync(user, cancellationToken);
            await _userSettingsRepository.AddAsync(userSettings, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            var response = new CreateUserOutput
            {
                Id = user.Id
            };

            return Result<CreateUserOutput>.Success(response);
        }
        catch (Exception ex)
        {
            

            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"Erro ao persistir usuário no banco de dados: {user.Email}, EXCEPTION: {ex.Message} INNER: {ex.InnerException}", input.Username);
            
            try
            {
                // Tenta remover o usuário do Keycloak se o commit falhar (compensação)
                await _keycloakService.DeleteUserAsync(keycloakUserId, cancellationToken);
            }
            catch (Exception kcEx)
            {
                _logger.LogError(kcEx, $"Falha ao tentar remover usuário do Keycloak após erro de commit: userId: {user.Id} username: {user.Username}");
            }

            return Result<CreateUserOutput>.Failure(
                Messages.UserCreationFailed,
                ErrorType.InternalError
            );
        }
    }

}
