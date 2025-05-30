using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities;

public class ExpenseShare : BaseEntity
{
    public Guid Id { get; private set; }
    public Guid ExpenseId { get; private set; }
    public Expense Expense { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public decimal Amount { get; private set; }

    internal ExpenseShare() { }

    internal ExpenseShare(Guid expenseId, Guid userId, decimal amount)
    {
        SetExpense(expenseId);
        SetUser(userId);
        SetAmount(amount);
    }

    public ExpenseShare SetExpense(Guid expenseId)
    {
        if (expenseId == Guid.Empty)
            throw new ArgumentException("Despesa inválida.");
        ExpenseId = expenseId;
        return this;
    }

    public ExpenseShare SetUser(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("Usuário inválido.");
        UserId = userId;
        return this;
    }

    public ExpenseShare SetAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "O valor da cota deve ser maior que zero.");
        Amount = amount;
        return this;
    }
}

