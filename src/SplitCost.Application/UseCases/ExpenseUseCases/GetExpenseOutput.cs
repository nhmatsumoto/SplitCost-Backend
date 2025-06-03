using SplitCost.Domain.Enums;

namespace SplitCost.Application.UseCases.ExpenseUseCases;

public class GetExpenseOutput
{
    public Guid Id { get; private set; }
    public ExpenseType Type { get; private set; }
    public ExpenseCategory Category { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    public Guid ResidenceId { get; private set; }
    public Guid RegisteredByUserId { get; private set; }
    public Guid PaidByUserId { get; private set; }
}
