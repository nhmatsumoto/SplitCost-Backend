using FluentValidation;
using SplitCost.Application.Common.Interfaces.Identity;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos.AppUser;
using SplitCost.Domain.Enums;

namespace SplitCost.Application.UseCases;

public class CreateApplicationUserUseCase : BaseUseCase<CreateApplicationUserInput, CreateApplicationUserOutput>
{
    private readonly IValidator<CreateApplicationUserInput> _validator;
    private readonly IKeycloakAuthService _keycloakAuthService;
    private readonly IKeycloakUserService _keycloakUserService;

    public CreateApplicationUserUseCase(
        IValidator<CreateApplicationUserInput> validator,
        IKeycloakAuthService keycloakAuthService,
        IKeycloakUserService keycloakUserService)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _keycloakAuthService = keycloakAuthService ?? throw new ArgumentNullException(nameof(keycloakAuthService));
        _keycloakUserService = keycloakUserService ?? throw new ArgumentNullException(nameof(keycloakUserService));
    }

    protected override async Task<Result<CreateApplicationUserOutput>> HandleAsync(
        CreateApplicationUserInput input,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(input, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<CreateApplicationUserOutput>.FromFluentValidation(
                "Houve um erro ao cadastrar o usuário",
                validationResult.Errors
            );
        }

        try
        {
            var userId = await _keycloakUserService.CreateUserAsync(
                input.Username,
                input.FirstName,
                input.LastName,
                input.Email,
                input.Password,
                cancellationToken
            );
            
            var roleName = input.Profile switch
            {
                UserProfileType.Member => "Member",
                UserProfileType.Administrator => "Admin",
                _ => throw new ArgumentOutOfRangeException(nameof(input.Profile), "Perfil inválido")
            };

            await _keycloakUserService.AssignRoleAsync(userId.ToString(), roleName, cancellationToken);

            var output = new CreateApplicationUserOutput
            {
                Id = userId,
                Username = input.Username,
                Email = input.Email,
                Profile = input.Profile
            };

            return Result<CreateApplicationUserOutput>.Success(output);
        }
        catch (Exception ex)
        {
            return Result<CreateApplicationUserOutput>.Failure(
                $"Erro ao criar usuário no Keycloak: {ex.Message}",
                ErrorType.InternalError
            );
        }
    }
}
