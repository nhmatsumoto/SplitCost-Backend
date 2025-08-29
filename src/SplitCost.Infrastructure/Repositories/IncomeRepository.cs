using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;

namespace SplitCost.Infrastructure.Repositories;

public class IncomeRepository : Repository<Income>, IIncomeRepository
{
    private readonly SplitCostDbContext _context;

    public IncomeRepository(SplitCostDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    
}
