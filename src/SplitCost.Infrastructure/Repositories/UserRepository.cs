using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;
using SplitCost.Infrastructure.Entities;

namespace SplitCost.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SplitCostDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(SplitCostDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task AddAsync(User user)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        await _context.Users.AddAsync(userEntity);
    }
    public async Task<User?> GetByIdAsync(Guid userId)
    {
        var userEntity = await _context.Users
            .Include(u => u.Residences)
                .ThenInclude(rm => rm.Residence)
            .Include(u => u.ResidenceExpensesPaid)
            .Include(u => u.ExpenseShares)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return _mapper.Map<User>(userEntity);
    }
        
    public void Update(User user)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        _context.Users.Update(userEntity);
    }
    public async Task<bool> ExistsAsync(Guid userId)
        => await _context.Users
            .AnyAsync(u => u.Id == userId);
}
