using Microsoft.AspNetCore.Mvc;
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

        public ExpenseController(ICreateExpenseUseCase createExpenseUseCase, IReadExpenseUseCase readExpenseUseCase)
        {
            _createExpenseUseCase = createExpenseUseCase ?? throw new ArgumentNullException(nameof(createExpenseUseCase));
            _readExpenseUseCase = readExpenseUseCase ?? throw new ArgumentNullException(nameof(readExpenseUseCase));
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
    }
}
