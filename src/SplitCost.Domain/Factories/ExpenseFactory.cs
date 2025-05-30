using SplitCost.Domain.Entities;
using SplitCost.Domain.Enums;

namespace SplitCost.Domain.Factories;

public static class ExpenseFactory
{
    public static Expense Create(
        ExpenseType type,
        ExpenseCategory category,
        decimal amount,
        DateTime date,
        string description,
        bool isSharedAmongMembers,
        Guid residenceId,
        Guid registeredByUserId,
        Guid paidByUserId)
    {
        return new Expense(type, category, amount, date, description, isSharedAmongMembers, residenceId, registeredByUserId, paidByUserId);
    }
}
