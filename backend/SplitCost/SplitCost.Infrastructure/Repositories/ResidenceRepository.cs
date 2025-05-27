using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Infrastructure.Repositories;

public class ResidenceRepository : IResidenceRepository
{
    private readonly SplitCostDbContext _context;

    public ResidenceRepository(SplitCostDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(Residence residence)
    {
        await _context.Residences.AddAsync(residence);
        await _context.SaveChangesAsync();
    }

    public async Task<Residence?> GetByIdAsync(Guid id)
    {
        return await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Include(r => r.Expenses)
                .ThenInclude(e => e.Shares)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Residence?> GetByUserIdAsync(Guid id)
    {
        return await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Where(r => r.Members.Any(m => m.UserId == id))
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Residence residence)
    {
        _context.Residences.Update(residence);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Residence>> GetAllAsync()
    {
        return await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Include(r => r.Expenses)
                .ThenInclude(e => e.Shares)
            .ToListAsync();
    }

    public async Task<Boolean> UserHasResidence(Guid userId)
    {
        return await _context.Residences
       .AnyAsync(r => r.Members.Any(m => m.UserId == userId));
    }
}
