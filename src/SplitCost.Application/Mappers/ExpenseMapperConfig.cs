using Mapster;
using SplitCost.Application.UseCases.CreateExpense;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.Mappers;
public class ExpenseMapperConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Input -> Domain
        config.NewConfig<CreateExpenseInput, Expense>()
            .MapWith(src => ExpenseFactory.Create(
                src.Type,
                src.Category,
                src.Amount,
                src.Date,
                src.Description,
                src.IsSharedAmongMembers,
                src.ResidenceId,
                src.RegisteredByUserId,
                src.PaidByUserId
            ));

        // Domain -> Output
        config.NewConfig<Expense, CreateExpenseOutput>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Category, src => src.Category)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Date, src => src.Date)
            .Map(dest => dest.Description, src => src.Description ?? "")
            .Map(dest => dest.IsSharedAmongMembers, src => src.IsSharedAmongMembers)
            .Map(dest => dest.ResidenceId, src => src.ResidenceId)
            .Map(dest => dest.RegisteredByUserId, src => src.RegisteredByUserId)
            .Map(dest => dest.PaidByUserId, src => src.PaidByUserId);
    }
}