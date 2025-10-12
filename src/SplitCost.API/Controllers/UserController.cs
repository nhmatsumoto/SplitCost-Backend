using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Dtos.AppUser;

namespace SplitCost.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register(CreateApplicationUserInput input)
    {
      
        // Implement registration logic here
        return Ok("Registration successful");
    }
}
