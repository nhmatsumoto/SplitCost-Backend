using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;

namespace SplitCost.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IAppUserUseCase _appUserUseCase;
    private readonly ICreateResidenceUseCase _createResidenceUseCase;

    public UsersController(IAppUserUseCase appUserUseCase, ICreateResidenceUseCase createResidenceUseCase)
    {
        _appUserUseCase = appUserUseCase;
        _createResidenceUseCase = createResidenceUseCase;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var userId = await _appUserUseCase.RegisterUserAsync(registerUserDto);

                if(userId != Guid.Empty)
                {
                    return Created();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return BadRequest(new
        {
            message = "Erro de validação",
            errors = ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            )
        });
    }
}
