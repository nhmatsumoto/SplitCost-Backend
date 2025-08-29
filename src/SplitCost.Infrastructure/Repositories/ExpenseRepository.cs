using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;

namespace SplitCost.Infrastructure.Repositories;

public class ExpenseRepository : Repository<Expense>, IExpenseRepository
{
    private readonly SplitCostDbContext _context;

    public ExpenseRepository(SplitCostDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    } 

}
