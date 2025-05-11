using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;

namespace SplitCost.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAppUserUseCase _appUserUseCase;

        public UsersController(IAppUserUseCase appUserUseCase)
        {
            _appUserUseCase = appUserUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var userId = await _appUserUseCase.RegisterUserAsync(dto.Name, dto.Email, dto.Password);
            return Ok(new { userId });
        }
    }
}
