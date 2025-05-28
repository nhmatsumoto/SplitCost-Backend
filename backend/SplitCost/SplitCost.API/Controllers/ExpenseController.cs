using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Common;
using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Application.UseCases;
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
        public async Task<IActionResult> Create([FromBody] CreateExpenseDto expenseDto)
        {
            if (expenseDto == null)
                return BadRequest();

            var result = await _createExpenseUseCase.CreateExpense(expenseDto);
            return CreatedAtAction(nameof(result), new { id = result.Id }, result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var expense = await _readExpenseUseCase.GetByIdAsync(id);
            if (expense == null)
                return NotFound();
            return Ok(expense);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByResidenceId(Guid id)
        {
            var expense = await _readExpenseUseCase.GetByResidenceIdAsync(id);
            if (expense == null)
                return NotFound();
            return Ok(expense);
        }

        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetExpenseCategories()
        {
            var categories = Enum.GetValues(typeof(ExpenseCategory))
                .Cast<ExpenseCategory>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            return Ok(categories);
        }

        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetExpenseTypes()
        {
            var types = Enum.GetValues(typeof(ExpenseType))
                .Cast<ExpenseType>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            return Ok(types);
        }


        // Revisar implementação, tratar erros adicinado um enum no result.
        [HttpGet("users/{residenceId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
