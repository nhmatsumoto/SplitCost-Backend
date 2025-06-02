using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Common;
using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Application.UseCases.CreateMember;
using SplitCost.Application.UseCases.CreateResidence;
using System.Security.Claims;

namespace SplitCost.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ResidencesController : ControllerBase
    {
       
        private readonly IUpdateResidenceUseCase _updateResidenceUseCase;
        private readonly IReadResidenceUseCase _getResidenceUseCase;

        private readonly IUseCase<AddResidenceMemberInput, Result<int>> _addResidenceMemberUseCase;
        private readonly IUseCase<CreateResidenceInput, Result<CreateResidenceOutput>> _createResidenceUseCase;
        public ResidencesController(
            IUseCase<CreateResidenceInput, Result<CreateResidenceOutput>> createResidenceUseCase,
            IUseCase<AddResidenceMemberInput, Result<int>> addResidenceMemberUseCase,
            IUpdateResidenceUseCase updateResidenceUseCase,
            IReadResidenceUseCase getResidenceUseCase)
        {
            _createResidenceUseCase = createResidenceUseCase ?? throw new ArgumentNullException(nameof(createResidenceUseCase));
            _updateResidenceUseCase = updateResidenceUseCase ?? throw new ArgumentNullException(nameof(updateResidenceUseCase));
            _getResidenceUseCase = getResidenceUseCase ?? throw new ArgumentNullException(nameof(getResidenceUseCase));
            _addResidenceMemberUseCase = addResidenceMemberUseCase ?? throw new ArgumentException(nameof(addResidenceMemberUseCase));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateResidence([FromBody] CreateResidenceInput reateResidenceInput)
        {

            try
            {

                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

                if (!Guid.TryParse(userIdStr, out var userId))
                    return Unauthorized(new { Error = "Usuário não autenticado corretamente." });

                var result = await _createResidenceUseCase.ExecuteAsync(reateResidenceInput);

                //Verificar se o usuário tem residência, levar isso para Validation Layer
                var hasResidence = await _getResidenceUseCase.UserHasResidence(userId);

                if (hasResidence)
                    return BadRequest(new { Error = "Usuário já possui uma residência." });


                if (result.IsSuccess)
                {
                    await _addResidenceMemberUseCase.ExecuteAsync(new CreateResidenceMemberDto
                    {
                        UserId = userId,
                        ResidenceId = result.Data.Id
                    });
                }

                return CreatedAtAction(nameof(GetResidence), new { id = result.Data.Id }, result.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{residenceId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateResidence(Guid residenceId, [FromBody] UpdateResidenceDto updateResidencecDto)
        {
            if (!ModelState.IsValid || residenceId != updateResidencecDto.ResidenceId)
                return BadRequest(ModelState);

            try
            {
                var residenceDto = await _updateResidenceUseCase.UpdateResidenceAsync(residenceId, updateResidencecDto);
                return Ok(residenceDto);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetResidence(Guid id)
        {
            try
            {
                var residenceDto = await _getResidenceUseCase.GetByIdAsync(id);
                if (residenceDto == null)
                    return NotFound(new { Error = "Residência não encontrada." });
                return Ok(residenceDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        [HttpGet("user/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetResidenceByUserId(Guid id)
        {
            try
            {
                var residenceDto = await _getResidenceUseCase.GetByUserIdAsync(id);
                if (residenceDto == null)
                    return NotFound(new { Error = "Residência não encontrada." });
                return Ok(residenceDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

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
