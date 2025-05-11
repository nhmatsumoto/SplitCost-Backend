namespace SplitCost.Domain.Entities
{
    public class ExpenseShare
    {
        public Guid Id { get; set; }

        public Guid ExpenseId { get; set; }
        public Expense Expense { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public decimal Amount { get; set; }
    }

}
