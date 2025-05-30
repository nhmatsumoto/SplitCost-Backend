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
        => await _context.Residences.AddAsync(residence);

    public async Task<Residence?> GetByIdAsync(Guid id)
        => await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Include(r => r.Expenses)
                .ThenInclude(e => e.Shares)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<Residence?> GetByUserIdAsync(Guid id)
        => await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Where(r => r.Members.Any(m => m.UserId == id))
            .AsNoTracking()
            .FirstOrDefaultAsync();
    public void UpdateAsync(Residence residence)
        => _context.Residences
        .Update(residence);
    public async Task<IEnumerable<Residence>> GetAllAsync()
        => await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Include(r => r.Expenses)
                .ThenInclude(e => e.Shares)
            .ToListAsync();
    public async Task<bool> UserHasResidence(Guid userId)
        => await _context.Residences
            .AnyAsync(r => r.Members
            .Any(m => m.UserId == userId));
    public async Task<bool> ExistsAsync(Guid id) 
        => await _context.Residences
            .AnyAsync(r => r.Id == id);
}
