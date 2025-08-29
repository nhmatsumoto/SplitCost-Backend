using FluentValidation;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.Services;
using SplitCost.Application.UseCases.Dtos;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases;

public class CreateApplicationUserUseCase(
    IUserRepository usuarioRepository,
    IKeycloakService keycloakService,
    IUnitOfWork unitOfWork,
    IValidator<CreateApplicationUserInput> validator,
    ILogger<CreateApplicationUserUseCase> logger) : IUseCase<CreateApplicationUserInput, Result<CreateApplicationUserOutput>>
{
    private readonly IUserRepository                        _usuarioRepository      = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
    private readonly IKeycloakService                       _keycloakService        = keycloakService   ?? throw new ArgumentNullException(nameof(keycloakService));
    private readonly IUnitOfWork                            _unitOfWork             = unitOfWork        ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IValidator<CreateApplicationUserInput> _validator              = validator         ?? throw new ArgumentNullException(nameof(validator));
    private readonly ILogger<CreateApplicationUserUseCase>  _logger                 = logger            ?? throw new ArgumentNullException(nameof(logger));
    
    public async Task<Result<CreateApplicationUserOutput>> ExecuteAsync(CreateApplicationUserInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var validationResult = await _validator.ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for CreateApplicationUserInput: {Errors}", validationResult.Errors);
            return Result<CreateApplicationUserOutput>.FromFluentValidation(
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
            return Result<CreateApplicationUserOutput>.Failure(
                "Erro ao criar usuário no Keycloak. Tente novamente mais tarde.",
                ErrorType.InternalError
            );
        }

        if (keycloakUserId == Guid.Empty)
        {
            return Result<CreateApplicationUserOutput>.Failure(
                "Erro ao criar usuário no Keycloak. Verifique os dados informados.",
                ErrorType.InternalError
            );
        }

        var name = $"{input.FirstName} {input.LastName}";

        var usuario = UserFactory.Create(
            keycloakUserId,
            input.Username,
            name,
            input.Email
        );

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _usuarioRepository.AddAsync(usuario, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            var response = new CreateApplicationUserOutput
            {
                Id = usuario.Id
            };

            return Result<CreateApplicationUserOutput>.Success(response);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Erro ao persistir usuário no banco de dados: {Username}", input.Username);

            // Tenta remover o usuário do Keycloak se o commit falhar (compensação)
            try
            {
                //await _keycloakService.DeleteUserAsync(keycloakUserId, cancellationToken);
            }
            catch (Exception kcEx)
            {
                _logger.LogError(kcEx, "Falha ao tentar remover usuário do Keycloak após erro de commit: {Username}", input.Username);
            }

            return Result<CreateApplicationUserOutput>.Failure(
                Messages.UserCreationFailed,
                ErrorType.InternalError
            );
        }
    }

}
