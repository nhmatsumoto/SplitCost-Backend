using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Infrastructure.Persistence.Mapping;

public static class IncomeEntityExtensions
{
    
    public static Income ToDomain(this Income entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return IncomeFactory.Create()
            .SetId(entity.Id)
            .SetCategory(entity.Category)
            .SetAmount(entity.Amount)
            .SetDate(entity.Date)
            .SetDescription(entity.Description)
            .SetResidenceId(entity.ResidenceId)
            .SetUserId(entity.UserId);
    }
}
