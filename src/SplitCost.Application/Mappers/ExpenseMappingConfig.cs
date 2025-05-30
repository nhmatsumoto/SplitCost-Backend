using Mapster;
using SplitCost.Application.DTOs;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.Mappers;

public class ExpenseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateExpenseDto, Expense>()
            .MapWith(src => new Expense(
                type: src.Type,
                category: src.Category,
                amount: src.Amount,
                date: src.Date,
                isSharedAmongMembers: src.IsSharedAmongMembers,
                description: src.Description,
                residenceId: src.ResidenceId,
                registeredByUserId: src.RegisterByUserId,
                paidByUserId: src.PaidByUserId
            ));
    }
}
