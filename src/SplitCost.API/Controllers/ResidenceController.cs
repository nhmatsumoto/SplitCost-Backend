using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.UseCases.Dtos;
using SplitCost.Domain.Entities;

namespace SplitCost.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ResidenceController : ControllerBase
    {
        private readonly IUseCase<CreateResidenceInput, Result<Residence>> _createResidenceUseCase;
        private readonly IUseCase<GetResidenceByIdInput, Result<Residence>> _getResidenceByIdUseCase;

        private readonly IUseCase<AddMemberInput, Result<Member>> _addMemberUseCase;
        private readonly IUseCase<GetMemberByResidenceIdInput, Result<GetMemberByresidenceIdOutput>> _getMemberUseCase;
        public ResidenceController(
            IUseCase<CreateResidenceInput, Result<Residence>> createResidenceUseCase,
            IUseCase<AddMemberInput, Result<Member>> addMemberUseCase,
            IUseCase<GetResidenceByIdInput, Result<Residence>> getResidenceByIdUseCase)
        {
            _createResidenceUseCase     = createResidenceUseCase    ?? throw new ArgumentNullException(nameof(createResidenceUseCase));
            _getResidenceByIdUseCase    = getResidenceByIdUseCase   ?? throw new ArgumentNullException(nameof(getResidenceByIdUseCase));
            _addMemberUseCase           = addMemberUseCase          ?? throw new ArgumentNullException(nameof(addMemberUseCase));
        }

        [HttpPost("create")]
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

            return CreatedAtAction(nameof(GetResidence), new { id = result.Data!.Id }, result);
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetResidence([FromQuery] GetResidenceByIdInput getResidenceByIdInput, CancellationToken cancellationToken)
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

        [HttpPost("member/create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddMember([FromBody] AddMemberInput addMemberInput, CancellationToken cancellationToken)
        {
            var result = await _addMemberUseCase.ExecuteAsync(addMemberInput, cancellationToken);

            if (!result.IsSuccess)
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(result),
                    ErrorType.Validation => BadRequest(result),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, result)
                };
            }

            return CreatedAtAction(nameof(GetMember), /*new { id = result.Data!.Id },*/ result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMember([FromBody] GetMemberByResidenceIdInput getMemberInput, CancellationToken cancellationToken)
        {

            var result = await _getMemberUseCase.ExecuteAsync(getMemberInput, cancellationToken);

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

    }
}
