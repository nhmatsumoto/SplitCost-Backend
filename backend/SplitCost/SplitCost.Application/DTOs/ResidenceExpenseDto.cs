using SplitCost.Domain.Entities;

namespace SplitCost.Application.DTOs
{
    public class ResidenceExpenseDto
    {
        public Guid Id { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }  // Ex: Mercado, Água, etc.
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsSharedAmongMembers { get; set; }
    }
}
