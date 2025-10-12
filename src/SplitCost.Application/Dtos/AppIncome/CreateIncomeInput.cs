using SplitCost.Domain.Enums;

namespace SplitCost.Application.Dtos.AppIncome
{
    public record CreateIncomeInput
    {
        public decimal Amount { get; set; }
        public IncomeCategory Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid ResidenceId { get; set; }
        public Guid UserId { get; set; }

        public CreateIncomeInput()
        {
                
        }
    }

}
