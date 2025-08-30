using SplitCost.Domain.Entities;
using SplitCost.Domain.Enums;

namespace SplitCost.Domain.Factories;
public static class ExpenseFactory
{
    public static Expense Create() => new Expense();

    public static Expense Create(
        ExpenseType type,
        ExpenseCategory category,
        decimal amount,
        DateTime date,
        string description,
        bool isSharedAmongMembers,
        Guid residenceId,
        Guid registeredByUserId,
        Guid paidByUserId) =>
            new Expense()
                .SetType(type)
                .SetCategory(category)
                .SetAmount(amount)
                .SetDate(date)
                .SetDescription(description)
                .SetResidenceId(residenceId);


    public static Expense Create(
       Guid id,
       ExpenseType type,
       ExpenseCategory category,
       decimal amount,
       DateTime date,
       string description,
       bool isSharedAmongMembers,
       Guid residenceId,
       Guid registeredByUserId,
       Guid paidByUserId) =>
           new Expense()
               .SetId(id)
               .SetType(type)
               .SetCategory(category)
               .SetAmount(amount)
               .SetDate(date)
               .SetDescription(description)
               .SetResidenceId(residenceId);
              
}
