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

        public async Task<Guid> RegisterUserAsync(string name, string email, string password)
        {
            var keycloakUserId = await _keycloakService.CreateUserAsync(email, name, password);

            var usuario = new User
            {
                Id = Guid.Parse(keycloakUserId),
                Name = name,
                Email = email
            };

            await _usuarioRepository.AddAsync(usuario);

            return usuario.Id;
        }

    }
}
