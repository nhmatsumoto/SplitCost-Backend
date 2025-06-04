using FluentValidation;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.Services;
using SplitCost.Domain.Exceptions;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases.ApplicationUserUseCases.CreateApplicationUser
{
    public class CreateApplicationUserUseCase : IUseCase<CreateApplicationUserInput, Result<CreateApplicationUserOutput>>
    {
        private readonly IUserRepository _usuarioRepository;
        private readonly IKeycloakService _keycloakService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateApplicationUserInput> _validator;
#warning TODO: Adicionar LOGS

        public CreateApplicationUserUseCase(
            IUserRepository usuarioRepository, 
            IKeycloakService keycloakService, 
            IUnitOfWork unitOfWork,
            IValidator<CreateApplicationUserInput> validator)
        {
            _usuarioRepository      = usuarioRepository     ?? throw new ArgumentNullException(nameof(usuarioRepository));
            _keycloakService        = keycloakService       ?? throw new ArgumentNullException(nameof(keycloakService));
            _unitOfWork             = unitOfWork            ?? throw new ArgumentNullException(nameof(unitOfWork));
            _validator              = validator             ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<Result<CreateApplicationUserOutput>> ExecuteAsync(CreateApplicationUserInput input, CancellationToken cancellationToken)
        {
            var validationResult = _validator.ValidateAsync(input, cancellationToken);

            if(!validationResult.Result.IsValid)
            {
                return Result<CreateApplicationUserOutput>.FromFluentValidation("Erro de validação", validationResult.Result.Errors);
            }

            var keycloakUserId = await _keycloakService.CreateUserAsync(
                input.Username,
                input.FirstName,
                input.LastName,
                input.Email,
                input.Password,
                cancellationToken
            );

            if (keycloakUserId != Guid.Empty)
            {
                var name = string.Concat(input.FirstName, " ", input.LastName);

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
                }
                catch (KeycloakInternalServerErrorException ex)
                {
                    await _unitOfWork.RollbackAsync(cancellationToken);
                    //_logger.LogError(ex, "Erro ao persistir usuário após criação no Keycloak");
                    return Result<CreateApplicationUserOutput>.Failure(Messages.UserCreationFailed, ErrorType.InternalError);
                }


                var response = new CreateApplicationUserOutput
                {
                    Id = usuario.Id,
                };

                return Result<CreateApplicationUserOutput>.Success(response);
            }

            return Result<CreateApplicationUserOutput>.Failure("Erro ao criar usuário no Keycloak. Verifique os dados informados e tente novamente.", ErrorType.InternalError);
        }

    }
}
