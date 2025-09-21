using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Expense>> GetExpenseListByResidenceId(Guid residenceId, CancellationToken cancellationToken)
    {
        return await _context.Expenses.Include(r => r.Residence).Where(x => x.ResidenceId == residenceId).ToListAsync(cancellationToken);
    }
}
