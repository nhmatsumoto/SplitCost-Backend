using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Common;
using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;

namespace SplitCost.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ICreateExpenseUseCase _createExpenseUseCase;
        private readonly IReadExpenseUseCase _readExpenseUseCase;
        private readonly IReadMemberUseCase _readMemberUseCase;
        public ExpenseController(
            ICreateExpenseUseCase createExpenseUseCase, 
            IReadExpenseUseCase readExpenseUseCase,
            IReadMemberUseCase readMemberUseCase)
        {
            _createExpenseUseCase = createExpenseUseCase ?? throw new ArgumentNullException(nameof(createExpenseUseCase));
            _readExpenseUseCase = readExpenseUseCase ?? throw new ArgumentNullException(nameof(readExpenseUseCase));
            _readMemberUseCase = readMemberUseCase ?? throw new ArgumentNullException(nameof(readMemberUseCase));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateExpenseDto expenseDto)
        {
            if (expenseDto == null)
            {
                return BadRequest();
            }

            var result = await _createExpenseUseCase.CreateExpense(expenseDto);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { id = ((ExpenseDto)result.Data!).Id }, result.Data);
            }
            else
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(new { Message = result.ErrorMessage }),
                    ErrorType.Validation => BadRequest(new { Message = result.ErrorMessage }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." }) 
                };
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _readExpenseUseCase.GetByIdAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            else
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(new { Message = result.ErrorMessage }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." })
                };
            }
        }

        [HttpGet("/residence/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByResidenceId(Guid id)
        {
            var result = await _readExpenseUseCase.GetByResidenceIdAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            else
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(new { Message = result.ErrorMessage }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred." })
                };
            }
        }

        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetExpenseCategories()
        {
            return Ok(Enum.GetValues(typeof(ExpenseCategory))
               .Cast<ExpenseCategory>()
               .Select(e => new { Value = (int)e, Name = e.ToString() })
               .ToList());
        }

        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetExpenseTypes()
        {
            return Ok(Enum.GetValues(typeof(ExpenseType))
                .Cast<ExpenseType>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList());
        }

        [HttpGet("users/{residenceId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExpenseUsers(Guid residenceId)
        {
            var result = await _readMemberUseCase.GetByResidenceIdAsync(residenceId);

            if (result.IsError)
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(new { success = false, error = result.ErrorMessage }),
                    ErrorType.Validation => BadRequest(new { success = false, error = result.ErrorMessage }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new { success = false, error = "Erro inesperado" })
                };
            }

            return Ok(result.Data);
        }
    }
}
