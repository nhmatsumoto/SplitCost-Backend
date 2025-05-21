using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using System.Security.Claims;

namespace SplitCost.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ResidencesController : ControllerBase
    {
        private readonly ICreateResidenceUseCase _createResidenceUseCase;
        private readonly IUpdateResidenceUseCase _updateResidenceUseCase;
        private readonly IReadResidenceUseCase _getResidenceUseCase;
        private readonly IRegisterResidenceOwnerUseCase _registerOwnerUseCase;

        public ResidencesController(
            ICreateResidenceUseCase createResidenceUseCase,
            IUpdateResidenceUseCase updateResidenceUseCase,
            IReadResidenceUseCase getResidenceUseCase,
            IRegisterResidenceOwnerUseCase registerOwnerUseCase)
        {
            _createResidenceUseCase = createResidenceUseCase;
            _updateResidenceUseCase = updateResidenceUseCase;
            _getResidenceUseCase = getResidenceUseCase;
            _registerOwnerUseCase = registerOwnerUseCase;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateResidence([FromBody] CreateResidenceDto createResidenceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
                var username = User.Identity?.Name ?? "unknown";
                var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";

                if (!Guid.TryParse(userIdStr, out var userId))
                    return Unauthorized(new { Error = "Usuário não autenticado corretamente." });

                var residenceDto = await _createResidenceUseCase.CreateResidenceAsync(createResidenceDto, userId);

                //Registra o usuário logado como proprietário da residência
                await _registerOwnerUseCase.RegisterResidenceOwnerAsync(new RegisterOwnerDto
                {
                    UserId = userId,
                    Username = username,
                    Email = email,
                    ResidenceId = residenceDto.Id
                });

                return CreatedAtAction(nameof(GetResidence), new { id = residenceDto.Id }, residenceDto);
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
