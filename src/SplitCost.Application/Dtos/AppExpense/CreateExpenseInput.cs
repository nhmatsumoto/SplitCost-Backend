using SplitCost.Domain.Enums;

namespace SplitCost.Application.Dtos.AppExpense;

public record CreateExpenseInput
{
    public ExpenseType Type { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid PaidByUserId { get; set; }
    public bool IsSharedAmongMembers { get; set; } = false;
    public Guid ResidenceId { get; set; }
    public List<Guid>? SharedWithUserIds { get; set; }
}
