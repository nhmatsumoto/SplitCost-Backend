using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;

namespace SplitCost.API.Controllers;

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
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        // Criar validação com fluent Validation para validar informações de criação do usuário

        if(registerUserDto.Password == registerUserDto.ConfirmPassword)
        {
            try
            {
                var userId = await _appUserUseCase.RegisterUserAsync(registerUserDto);
                return Ok(new { userId });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return BadRequest();
    }
}
