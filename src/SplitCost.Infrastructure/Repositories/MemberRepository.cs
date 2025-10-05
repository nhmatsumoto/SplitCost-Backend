using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Context;

namespace SplitCost.Infrastructure.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    private readonly SplitCostDbContext _context;

    public MemberRepository(SplitCostDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

}
