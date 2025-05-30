using Mapster;
using SplitCost.Application.DTOs;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.Mappers;
public class ExpenseDomainMapperConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // DTO -> Domain
        config.NewConfig<CreateExpenseDto, Expense>()
            .MapWith(src => ExpenseFactory.Create(
                src.Type,
                src.Category,
                src.Amount,
                src.Date,
                src.Description,
                src.IsSharedAmongMembers,
                src.ResidenceId,
                src.RegisterByUserId,
                src.PaidByUserId
            ));
    }
}