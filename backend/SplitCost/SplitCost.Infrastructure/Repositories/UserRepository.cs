using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Repository;

namespace SplitCost.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SplitCostDbContext _context;

    public UserRepository(SplitCostDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _context.Users
            .Include(u => u.Residences)
                .ThenInclude(rm => rm.Residence)
            .Include(u => u.ExpensesPaid)
            .Include(u => u.ExpenseShares)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
