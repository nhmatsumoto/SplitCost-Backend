using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SplitCost.Infrastructure.Persistence.Mapping;

public static class ExpenseEntityExtensions
{
    /// <summary>
    /// Converte uma instância de ExpenseEntity para uma instância de Expense do domínio.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Expense ToDomain(this ExpenseEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return ExpenseFactory.Create()
            .SetId(entity.Id)
            .SetType(entity.Type)
            .SetCategory(entity.Category)
            .SetAmount(entity.Amount)
            .SetDate(entity.Date)
            .SetDescription(entity.Description)
            .SetSharedAmongMembers(entity.IsSharedAmongMembers)
            .SetResidenceId(entity.ResidenceId)
            .SetWhoRegistered(entity.RegisteredByUserId)
            .SetWhoPaid(entity.PaidByUserId);
    }
   
    //public static ExpenseEntity ToEntity(this Expense expense)
    //{
    //    if (expense == null) throw new ArgumentNullException(nameof(expense));

    //    return new ExpenseEntity
    //    {
    //       Id = expense.Id,

    //    };
    //}
}
