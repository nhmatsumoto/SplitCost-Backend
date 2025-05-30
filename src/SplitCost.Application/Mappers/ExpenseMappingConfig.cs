using Mapster;
using SplitCost.Application.DTOs;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.Mappers;
public class ExpenseMappingConfig : IRegister
{
    /// <summary>
    /// Registra o mapeamento entre CreateExpenseDto e Expense.
    /// Atenção a ordem dos parâmetros no construtor
    /// </summary>
    public void Register(TypeAdapterConfig config)
    {
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