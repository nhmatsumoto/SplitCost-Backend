using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Dtos.AppUser;

namespace SplitCost.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUseCase<CreateApplicationUserInput, Result<CreateApplicationUserOutput>> _createApplicationUser;
    
    public UserController(IUseCase<CreateApplicationUserInput, Result<CreateApplicationUserOutput>> createApplicationUser)
    {
        _createApplicationUser = createApplicationUser ?? throw new ArgumentNullException(nameof(createApplicationUser));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateApplicationUserInput input, CancellationToken cancellationToken)
    {

        var result = await _createApplicationUser.ExecuteAsync(input, cancellationToken);

        if(!result.IsSuccess)
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(result),
                ErrorType.Validation => BadRequest(result),
                _ => StatusCode(StatusCodes.Status500InternalServerError, result)
            };
        }

        return Ok();
    }

}
