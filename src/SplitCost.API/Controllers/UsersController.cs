using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Common;
using SplitCost.Application.Interfaces;
using SplitCost.Application.UseCases.CreateApplicationUser;
using SplitCost.Application.UseCases.GetApplicationUser;

namespace SplitCost.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUseCase<CreateApplicationUserInput, Result<CreateApplicationUserOutput>> _createApplicationUserUseCase;
    private readonly IUseCase<Guid, Result<GetApplicationUserByIdOutput>> _getApplicationUserUseCase;
    
    public UsersController(
        IUseCase<CreateApplicationUserInput, Result<CreateApplicationUserOutput>> createApplicationUserUseCase,
        IUseCase<Guid, Result<GetApplicationUserByIdOutput>> getApplicationUserUseCase)
    {
        _createApplicationUserUseCase   = createApplicationUserUseCase  ?? throw new ArgumentNullException(nameof(createApplicationUserUseCase));
        _getApplicationUserUseCase      = getApplicationUserUseCase     ?? throw new ArgumentNullException(nameof(getApplicationUserUseCase));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterApplicationUser([FromBody] CreateApplicationUserInput createApplicationUserInput)
    {

        var result = await _createApplicationUserUseCase.ExecuteAsync(createApplicationUserInput);
        if (!result.IsSuccess)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(result),
                ErrorType.Validation => BadRequest(result),
                _ => StatusCode(StatusCodes.Status500InternalServerError, result)
            };
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid userIdInput)
    {
        var result = await _getApplicationUserUseCase.ExecuteAsync(userIdInput);

        if (!result.IsSuccess)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(result),
                _ => StatusCode(StatusCodes.Status500InternalServerError, result)
            };
            
        }

        return Ok(result);
    }

}
