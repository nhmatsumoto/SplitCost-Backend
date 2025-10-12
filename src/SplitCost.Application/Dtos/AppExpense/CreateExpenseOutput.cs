using SplitCost.Domain.Enums;

namespace SplitCost.Application.Dtos.AppExpense;

public record CreateExpenseOutput
{
    public Guid Id { get; set; }
    public ExpenseType Type { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool IsSharedAmongMembers { get; set; }
    public string? Description { get; set; }
    public Guid ResidenceId { get; set; }
    public Guid RegisteredByUserId { get; set; }
    public Guid PaidByUserId { get; set; }
}
