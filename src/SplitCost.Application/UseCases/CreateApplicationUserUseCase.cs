using SplitCost.Application.Common.Services;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos;

namespace SplitCost.Application.UseCases
{
    public class CreateApplicationUserUseCase : BaseUseCase<CreateApplicationUserInput, CreateApplicationUserOutput>
    {
        public readonly IKeycloakService _keycloakService;
        public CreateApplicationUserUseCase(IKeycloakService keycloakService)
        {
            _keycloakService = keycloakService ?? throw new ArgumentNullException(nameof(keycloakService));
        }



    }
}
