using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Persistence.Mapping;

namespace SplitCost.Infrastructure.Repositories;

public class ResidenceRepository : IResidenceRepository
{
    private readonly SplitCostDbContext _context;

    public ResidenceRepository(SplitCostDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Residence> AddAsync(Residence residence, CancellationToken cancellationToken)
    {
        var entity = residence.ToEntity();
        var result = await _context.Residences.AddAsync(entity);
        return result.Entity.ToDomain();
    }

    public async Task<Residence?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Include(r => r.Expenses)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return entity?.ToDomain();
    }

    public async Task<Residence?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entity = await _context.Residences
            .Where(x => x.CreatedByUserId == userId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return entity?.ToDomain();
    }

    public void Update(Residence residence)
    {
        var entry = residence.ToEntity();
        _context.Residences.Update(entry);
    }

    public async Task<IEnumerable<Residence>> GetAllAsync(CancellationToken cancellationToken)
    {

        var entity = await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Include(r => r.Expenses)
            .ToListAsync(cancellationToken);

        return entity.Select(e => e.ToDomain());
    }

    public async Task<bool> UserHasResidence(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Residences
            .AnyAsync(r => r.Members.Any(m => m.UserId == userId), cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Residences
            .AnyAsync(r => r.Id == id, cancellationToken);
    }
}
