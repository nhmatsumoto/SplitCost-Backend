namespace SplitCost.Domain.Entities
{
    public class Expense
    {
        public Guid Id { get; set; }

        public Guid ResidenceId { get; set; }
        public Residence Residence { get; set; } = null!;

        public ExpenseType Type { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public Guid PaidByUserId { get; set; }
        public User PaidByUser { get; set; } = null!;

        public bool IsShared { get; set; }

        public ICollection<ExpenseShare> Shares { get; set; } = new List<ExpenseShare>();
    }

}
