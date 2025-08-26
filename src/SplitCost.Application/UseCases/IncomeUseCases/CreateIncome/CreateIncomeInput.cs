namespace SplitCost.Application.UseCases.IncomeUseCases.CreateIncome
{
    public class CreateIncomeInput
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Guid ResidenceId { get; set; }
        public Guid RegisteredByUserId { get; set; }
    }
}
