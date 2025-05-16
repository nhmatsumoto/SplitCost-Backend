using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases
{
    public class CreateAppUserUseCase : IAppUserUseCase
    {
        private readonly IUserRepository _usuarioRepository;
        private readonly IKeycloakService _keycloakService;

        public CreateAppUserUseCase(IUserRepository usuarioRepository, IKeycloakService keycloakService)
        {
            _usuarioRepository = usuarioRepository;
            _keycloakService = keycloakService;
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

            var usuario = new User
            {
                Id = Guid.Parse(keycloakUserId),
                Name = string.Concat(registerUserDto.FirstName, " ", registerUserDto.LastName),
                Email = registerUserDto.Email
            };

            await _usuarioRepository.AddAsync(usuario);

            return usuario.Id;
        }

    }
}
