using Mapster;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SplitCost.Infrastructure.Mappers;
public class ExpenseEntityMapperConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Domain -> Entity
        config.NewConfig<Expense, ExpenseEntity>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Category, src => src.Category)
            .Map(dest => dest.Description, src => src.Description ?? "")
            .Map(dest => dest.PaidByUserId, src => src.PaidByUserId)
            .Map(dest => dest.RegisteredByUserId, src => src.RegisteredByUserId)
            .Map(dest => dest.ResidenceId, src => src.ResidenceId)
            .Map(dest => dest.Date, src => src.Date);

        // Entity -> Domain
        config.NewConfig<ExpenseEntity, Expense>()
            .MapWith(src => ExpenseFactory.Create(
                src.Type,
                src.Category,
                src.Amount,
                src.Date,
                src.Description ?? string.Empty,
                src.IsSharedAmongMembers,
                src.ResidenceId,
                src.RegisteredByUserId,
                src.PaidByUserId
            ).SetId(src.Id));
    }
}
