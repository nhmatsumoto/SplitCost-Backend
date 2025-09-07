using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Dtos;
using SplitCost.Domain.Entities;

namespace SplitCost.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUseCase<CreateUserInput, Result<CreateUserOutput>> _createApplicationUserUseCase;
    private readonly IUseCase<Guid, Result<User>> _getApplicationUserUseCase;
    private readonly IUseCase<Guid, Result<UserSettings>> _getUserSettingsUseCase;

    public UserController(
        IUseCase<CreateUserInput, Result<CreateUserOutput>> createApplicationUserUseCase,
        IUseCase<Guid, Result<User>> getApplicationUserUseCase,
        IUseCase<Guid, Result<UserSettings>> getUserSettingsUseCase)
    {
        _createApplicationUserUseCase   = createApplicationUserUseCase  ?? throw new ArgumentNullException(nameof(createApplicationUserUseCase));
        _getApplicationUserUseCase      = getApplicationUserUseCase     ?? throw new ArgumentNullException(nameof(getApplicationUserUseCase));
        _getUserSettingsUseCase         = getUserSettingsUseCase        ?? throw new ArgumentNullException(nameof(getUserSettingsUseCase));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterApplicationUser([FromBody] CreateUserInput createApplicationUserInput, CancellationToken cancellationToken)
    {
        var result = await _createApplicationUserUseCase.ExecuteAsync(createApplicationUserInput, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(result),
                ErrorType.Validation => BadRequest(result),
                _ => StatusCode(StatusCodes.Status500InternalServerError, result)
            };
        }

        return CreatedAtAction(nameof(GetById), new { userId = result.Data?.Id }, result);
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _getApplicationUserUseCase.ExecuteAsync(userId, cancellationToken);

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

    [HttpGet("settings/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserSettings(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await _getUserSettingsUseCase.ExecuteAsync(userId, cancellationToken);

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
