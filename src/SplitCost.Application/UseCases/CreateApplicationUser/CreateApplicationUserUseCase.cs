using FluentValidation;
using SplitCost.Application.Common;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Factories;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases.CreateApplicationUser
{
    public class CreateApplicationUserUseCase : IUseCase<CreateApplicationUserInput, Result<CreateApplicationUserOutput>>
    {
        private readonly IUserRepository _usuarioRepository;
        private readonly IKeycloakService _keycloakService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateApplicationUserInput> _validator;

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

        public async Task<Result<CreateApplicationUserOutput>> ExecuteAsync(CreateApplicationUserInput input)
        {
            var validationResult = _validator.ValidateAsync(input);

            if(!validationResult.Result.IsValid)
            {
                return Result<CreateApplicationUserOutput>.FromFluentValidation("Erro de validação", validationResult.Result.Errors);
            }

            var keycloakUserId = await _keycloakService.CreateUserAsync(
                input.Username,
                input.FirstName,
                input.LastName,
                input.Email,
                input.Password
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

                await _usuarioRepository.AddAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

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
