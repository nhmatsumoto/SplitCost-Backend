using Microsoft.EntityFrameworkCore;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Context;
using SplitCost.Infrastructure.Persistence.Mapping;

namespace SplitCost.Infrastructure.Repositories;

public class ResidenceRepository : Repository<Residence>, IResidenceRepository
{
    private readonly SplitCostDbContext _context;

    public ResidenceRepository(SplitCostDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> UserHasResidence(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Residences.AnyAsync(r => r.Members.Any(m => m.UserId == userId), cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Residences.AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Residence?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entity = await _context.Residences
            .Include(r => r.Members)
            .Include(r => r.Expenses)
            .Where(r => r.Members.Any(m => m.UserId == userId))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return entity?.ToDomain();
    }

    public async Task<Residence?> GetByInviteCodeAsync(string inviteCode)
    {
        if (string.IsNullOrWhiteSpace(inviteCode))
            return null;

        var code = inviteCode.Trim().ToUpper();
        return await _context.Residences
            .Include(r => r.Members)
            .Include(r => r.Incomes)
            .Include(r => r.Expenses)
            .FirstOrDefaultAsync(r => r.InviteCode == code);
    }

}
