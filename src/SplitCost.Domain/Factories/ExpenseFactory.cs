using SplitCost.Domain.Entities;
using SplitCost.Domain.Enums;

namespace SplitCost.Domain.Factories;
public static class ExpenseFactory
{
    /// <summary>
    /// Cria uma instância de Expense com os parâmetros fornecidos.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="category"></param>
    /// <param name="amount"></param>
    /// <param name="date"></param>
    /// <param name="description"></param>
    /// <param name="isSharedAmongMembers"></param>
    /// <param name="residenceId"></param>
    /// <param name="registeredByUserId"></param>
    /// <param name="paidByUserId"></param>
    /// <returns></returns>
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
