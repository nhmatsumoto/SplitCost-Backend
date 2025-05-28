using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SplitCostDbContext _context;

    public UserRepository(SplitCostDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(User user)
        => await _context.Users.AddAsync(user);

    public async Task<User?> GetByIdAsync(Guid userId)
        => await _context.Users
            .Include(u => u.Residences)
                .ThenInclude(rm => rm.Residence)
            .Include(u => u.ResidenceExpensesPaid)
            .Include(u => u.ExpenseShares)
            .FirstOrDefaultAsync(u => u.Id == userId);

    public void Update(User user)
        => _context.Users.Update(user);
}
