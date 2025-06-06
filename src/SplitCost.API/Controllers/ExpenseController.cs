﻿using Microsoft.AspNetCore.Mvc;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.UseCases.ExpenseUseCases.CreateExpense;
using SplitCost.Application.UseCases.GetExpense;
using SplitCost.Domain.Enums;

namespace SplitCost.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IUseCase<CreateExpenseInput, Result<CreateExpenseOutput>> _createExpenseUseCase;
        private readonly IUseCase<Guid, Result<GetExpenseByIdOutput>> _getExpenseByIdUseCase;
        private readonly IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>> _getExpenseByResidenceIdUseCase;
        private readonly IUseCase<Guid, Result<Dictionary<Guid, string>>> _getMemberByResidenceIdUseCase;
      
        public ExpenseController(
            IUseCase<CreateExpenseInput, Result<CreateExpenseOutput>> createExpenseUseCase,
            IUseCase<Guid, Result<GetExpenseByIdOutput>> getExpenseByIdUseCase,
            IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>> getExpenseByResidenceIdUseCase,
            IUseCase<Guid, Result<Dictionary<Guid, string>>> getMemberByResidenceIdUseCase)
        {
            _createExpenseUseCase           = createExpenseUseCase              ?? throw new ArgumentNullException(nameof(createExpenseUseCase));
            _getExpenseByIdUseCase          = getExpenseByIdUseCase             ?? throw new ArgumentNullException(nameof(getExpenseByIdUseCase));
            _getExpenseByResidenceIdUseCase = getExpenseByResidenceIdUseCase    ?? throw new ArgumentNullException(nameof(getExpenseByResidenceIdUseCase));
            _getMemberByResidenceIdUseCase  = getMemberByResidenceIdUseCase     ?? throw new ArgumentNullException(nameof(getMemberByResidenceIdUseCase));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateExpenseInput createExpenseInput, CancellationToken cancellationToken)
        {
            var result = await _createExpenseUseCase.ExecuteAsync(createExpenseInput, cancellationToken);

            if (!result.IsSuccess)
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(result),
                    ErrorType.Validation => BadRequest(result),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, result)
                };
            }

            return CreatedAtAction(nameof(GetById), new { id = ((CreateExpenseOutput)result.Data!).Id }, result);
        }


        //TODO ADICIONAR VALIDATOR PARA TRATAR ID do Expense
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid expenseIdInput, CancellationToken cancellationToken)
        {
            var result = await _getExpenseByIdUseCase.ExecuteAsync(expenseIdInput, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(result),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, result)
                };
            }
        }

        //ADICIONAR VALIDATOR PARA TRATAR ID DO RESIDENCE
        [HttpGet("residence/{residenceId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExpensesByResidenceId(Guid residenceIdInput, CancellationToken cancellationToken)
        {
            var result = await _getExpenseByResidenceIdUseCase.ExecuteAsync(residenceIdInput, cancellationToken);

            if (!result.IsSuccess)
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(result),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, result)
                };
            }

            return Ok(result.Data);
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


        //ADICIONAR VALIDATOR PARA TRATAR ID DO RESIDENCE
        [HttpGet("users/{residenceId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsersByResidenceId(Guid residenceId, CancellationToken cancellationToken)
        {
            var result = await _getMemberByResidenceIdUseCase.ExecuteAsync(residenceId, cancellationToken);

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
    }
}
