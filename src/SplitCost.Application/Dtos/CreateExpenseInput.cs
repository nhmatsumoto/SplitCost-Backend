using SplitCost.Domain.Enums;

namespace SplitCost.Application.Dtos;

public record CreateExpenseInput
{
    public ExpenseType Type { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid PaidByUserId { get; set; }
    public bool IsSharedAmongMembers { get; set; } = false;
    public List<Guid>? SharedWithUserIds { get; set; }
}
