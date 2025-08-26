using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SplitCost.Infrastructure.Persistence.Mapping;

public static class IncomeEntityExtensions
{
    
    public static Income ToDomain(this IncomeEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return IncomeFactory.Create()
            .SetId(entity.Id)
            .SetCategory(entity.Category)
            .SetAmount(entity.Amount)
            .SetDate(entity.Date)
            .SetDescription(entity.Description)
            .SetResidenceId(entity.ResidenceId)
            .SetRegisteredByUserId(entity.RegisteredByUserId);
    }
}
