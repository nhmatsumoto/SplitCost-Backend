using Microsoft.AspNetCore.Mvc;

namespace SplitCost.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        public IActionResult GetExpenseByResidenceId(Guid residenceId)
        {
            return Ok();
        }

        public IActionResult GetExpenseByUserId(Guid userId)
        {
            return Ok();
        }
    }
}
