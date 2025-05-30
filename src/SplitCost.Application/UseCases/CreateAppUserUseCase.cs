using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases
{
    public class CreateAppUserUseCase : IAppUserUseCase
    {
        private readonly IUserRepository _usuarioRepository;
        private readonly IKeycloakService _keycloakService;
        private readonly IUnitOfWork _unitOfWork;
        public CreateAppUserUseCase(
            IUserRepository usuarioRepository, 
            IKeycloakService keycloakService, 
            IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _keycloakService = keycloakService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var keycloakUserId = await _keycloakService.CreateUserAsync(
                registerUserDto.Username, 
                registerUserDto.FirstName, 
                registerUserDto.LastName, 
                registerUserDto.Email, 
                registerUserDto.Password
            );

            var userId = Guid.Parse(keycloakUserId);

            if (userId != Guid.Empty)
            {
                var name = string.Concat(registerUserDto.FirstName, " ", registerUserDto.LastName);

                var usuario = UserFactory.Create(
                    userId,
                    name,
                    registerUserDto.Email);

                await _usuarioRepository.AddAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

                return usuario.Id;
            }

            return Guid.Empty;
        }

    }
}
