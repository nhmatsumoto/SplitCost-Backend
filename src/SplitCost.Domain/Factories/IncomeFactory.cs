using SplitCost.Domain.Entities;
using SplitCost.Domain.Enums;

namespace SplitCost.Domain.Factories;
public static class IncomeFactory
{
    public static Income Create() => new Income();

    public static Income Create(
        decimal amount,
        IncomeCategory category,
        DateTime date,
        Guid residenceId,
        Guid userId,
        string description) =>
            new Income()
                .SetAmount(amount)
                .SetCategory(category)
                .SetDate(date)
                .SetDescription(description)
                .SetResidenceId(residenceId)
                .SetUserId(userId);

    public static Income Create(
       Guid id,
       decimal amount,
       IncomeCategory category,
       DateTime date,
       string description,
       Guid residenceId,
       Guid registeredByUserId) =>
           new Income()
                .SetId(id)
                .SetAmount(amount)
                .SetCategory(category)
                .SetDate(date)
                .SetDescription(description)
                .SetResidenceId(residenceId)
                .SetUserId(registeredByUserId);
}
