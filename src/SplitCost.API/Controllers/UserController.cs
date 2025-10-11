using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Common.Services;
using SplitCost.Application.Dtos;

namespace SplitCost.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    public readonly IKeycloakService _keycloakService;

    public UserController(IKeycloakService keycloakService)
    {
        _keycloakService = keycloakService;
    }

    [HttpPost("register")]
    public IActionResult Register(CreateUserInput input)
    {
        _keycloakService.CreateUserAsync();

        // Implement registration logic here
        return Ok("Registration successful");
    }
}
