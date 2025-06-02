using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Common;
using SplitCost.Application.Interfaces;
using SplitCost.Application.UseCases.CreateMember;
using SplitCost.Application.UseCases.CreateResidence;
using SplitCost.Application.UseCases.GetResidence;

namespace SplitCost.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ResidencesController : ControllerBase
    {
        private readonly IUseCase<AddResidenceMemberInput, Result<int>> _addResidenceMemberUseCase;
        private readonly IUseCase<CreateResidenceInput, Result<CreateResidenceOutput>> _createResidenceUseCase;
        private readonly IUseCase<GetResidenceByIdInput, Result<GetResidenceByIdOutput>> _getResidenceByIdUseCase;
        public ResidencesController(
            IUseCase<CreateResidenceInput, Result<CreateResidenceOutput>> createResidenceUseCase,
            IUseCase<AddResidenceMemberInput, Result<int>> addResidenceMemberUseCase,
            IUseCase<GetResidenceByIdInput, Result<GetResidenceByIdOutput>> getResidenceByIdUseCase)
        {
            _createResidenceUseCase     = createResidenceUseCase    ?? throw new ArgumentNullException(nameof(createResidenceUseCase));
            _getResidenceByIdUseCase    = getResidenceByIdUseCase   ?? throw new ArgumentNullException(nameof(getResidenceByIdUseCase));
            _addResidenceMemberUseCase  = addResidenceMemberUseCase ?? throw new ArgumentException(nameof(addResidenceMemberUseCase));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateResidence([FromBody] CreateResidenceInput reateResidenceInput, CancellationToken cancellationToken)
        {
            var result = await _createResidenceUseCase.ExecuteAsync(reateResidenceInput, cancellationToken);

            if (!result.IsSuccess)
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(result),
                    ErrorType.Validation => BadRequest(result),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, result)
                };
            }

            return CreatedAtAction(nameof(GetResidence), new { id = ((CreateResidenceOutput)result.Data!).Id }, result);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetResidence([FromBody]GetResidenceByIdInput getResidenceByIdInput, CancellationToken cancellationToken)
        {

            var result = await _getResidenceByIdUseCase.ExecuteAsync(getResidenceByIdInput, cancellationToken);

            if (!result.IsSuccess)
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(result),
                    ErrorType.Validation => BadRequest(result),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, result)
                };
            }

            return Ok(result);
        }

        //private Guid ParseGuid(string? value)
        //{
        //    if (string.IsNullOrEmpty(value) || !Guid.TryParse(value, out var guid))
        //        throw new ArgumentException("Valor inválido para Guid.", nameof(value));
        //    return guid;
        //}

        //[HttpPut("{residenceId:guid}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UpdateResidence(Guid residenceId, [FromBody] UpdateResidenceDto updateResidencecDto)
        //{
        //    if (!ModelState.IsValid || residenceId != updateResidencecDto.ResidenceId)
        //        return BadRequest(ModelState);

        //    try
        //    {
        //        var residenceDto = await _updateResidenceUseCase.UpdateResidenceAsync(residenceId, updateResidencecDto);
        //        return Ok(residenceDto);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return NotFound(new { Error = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Error = ex.Message });
        //    }
        //}




        //[HttpGet("user/{id:guid}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetResidenceByUserId(Guid id)
        //{
        //    try
        //    {
        //        var residenceDto = await _getResidenceUseCase.GetByUserIdAsync(id);
        //        if (residenceDto == null)
        //            return NotFound(new { Error = "Residência não encontrada." });
        //        return Ok(residenceDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Error = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetAllResidences()
        //{
        //    try
        //    {
        //        var residences = await _getResidenceUseCase.GetAllAsync();
        //        return Ok(residences);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Error = ex.Message });
        //    }
        //}
    }
}
