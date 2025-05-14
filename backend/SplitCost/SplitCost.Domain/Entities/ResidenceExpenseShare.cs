using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities
{
    public class ResidenceExpenseShare : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid ResidenceExpenseId { get; set; }
        public ResidenceExpense ResidenceExpense { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public decimal Amount { get; set; }
    }

}
