using SplitCost.Domain.Enums;

namespace SplitCost.Application.Dtos.AppExpense;

public record ExpenseDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public ExpenseCategory Category { get; set; }
    public ExpenseType Type { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid RegisteredByUserId { get; set; }
    public Guid PaidByUserId { get; set; }
}
