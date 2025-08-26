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
        string description) =>
            new Income()
                .SetAmount(amount)
                .SetCategory(category)
                .SetDate(date)
                .SetDescription(description)
                .SetResidenceId(residenceId);

    public static Income Create(
       Guid id,
       IncomeCategory category,
       DateTime date,
       string description) =>
           new Income()
                .SetId(id)
                .SetCategory(category)
                .SetDate(date)
                .SetDescription(description);
}
