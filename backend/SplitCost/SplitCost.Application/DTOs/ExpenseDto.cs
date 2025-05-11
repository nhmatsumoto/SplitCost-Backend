namespace SplitCost.Application.DTOs
{
    public class ExpenseDto
    {
        public Guid Id { get; set; }
        public string ExpenseType { get; set; }  // Ex: Mercado, Água, etc.
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
    }
}
