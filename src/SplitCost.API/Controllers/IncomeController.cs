using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.UseCases.IncomeUseCases.CreateIncome;

namespace SplitCost.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IUseCase<CreateIncomeInput, Result<CreateIncomeOutput>> _createIncomeUseCase;

        public IncomeController(IUseCase<CreateIncomeInput, Result<CreateIncomeOutput>> createIncomeUseCase)
        {
            _createIncomeUseCase = createIncomeUseCase ?? throw new ArgumentNullException(nameof(createIncomeUseCase));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateIncomeInput createIncomeInput, CancellationToken cancellationToken)
        {
            var result = await _createIncomeUseCase.ExecuteAsync(createIncomeInput, cancellationToken);
            if (!result.IsSuccess)
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(result),
                    ErrorType.Validation => BadRequest(result),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, result)
                };
            }
            return CreatedAtAction(nameof(Create), new { id = ((CreateIncomeOutput)result.Data!).Id }, result);
        }

    }

}
