using SplitCost.Domain.Enums;

namespace SplitCost.Application.UseCases.CreateExpense;

public class CreateExpenseInput
{
    public ExpenseType Type { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public Guid ResidenceId { get; set; }
    public Guid RegisteredByUserId { get; set; }
    public Guid PaidByUserId { get; set; }

    // Se a despesa for compartilhada
    public bool IsSharedAmongMembers { get; set; }

    // Lista dos membros que vão dividir a despesa
    public List<Guid>? SharedWithUserIds { get; set; }
}
