using SplitCost.Application.Dtos;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.Mappers;

public static class ExpenseMapper
{
    public static Expense ToDomain(this CreateExpenseInput input)
    {
        return ExpenseFactory.Create()
            .SetType(input.Type)
            .SetCategory(input.Category)
            .SetAmount(input.Amount)
            .SetDescription(input.Description);
    }

    public static CreateExpenseOutput ToOutput(this Expense domain, bool isSharedAmongMembers)
    {
        return new CreateExpenseOutput
        {
            Id = domain.Id,
            Type = domain.Type,
            Category = domain.Category,
            Amount = domain.Amount,
            Date = domain.Date,
            Description = domain.Description,
            ResidenceId = domain.ResidenceId,
            PaidByUserId = domain.PayingUserId,
            IsSharedAmongMembers = isSharedAmongMembers
        };
    }
}

