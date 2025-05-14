using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities
{
    public class ResidenceExpense : BaseEntity
    {
        public Guid Id { get; set; }
        public ResidenceExpenseType Type { get; set; }
        public ExpenseCategory Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsSharedAmongMembers { get; set; }
        public string Description { get; set; } = null!;

        // Navigation EF Core
        public Guid ResidenceId { get; set; }
        public Residence Residence { get; set; }

        public Guid RegisteredByUserId { get; set; }
        public User RegisteredBy { get; set; } = null!;

        public Guid PaidByUserId { get; set; }
        public User PaidBy { get; set; } = null!;

        public ICollection<ResidenceExpenseShare> Shares { get; set; } = new List<ResidenceExpenseShare>();
    }
}
