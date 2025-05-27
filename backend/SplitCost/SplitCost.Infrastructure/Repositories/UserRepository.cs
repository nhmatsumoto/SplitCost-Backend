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
    {
        try
        {
            await _context.Users.AddAsync(user);
        }catch(Exception ex)
        {
            // Log the exception or handle it as needed
            throw new Exception("An error occurred while adding the user.", ex);
        }

    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _context.Users
            .Include(u => u.Residences)
                .ThenInclude(rm => rm.Residence)
            .Include(u => u.ResidenceExpensesPaid)
            .Include(u => u.ExpenseShares)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
    }
}
