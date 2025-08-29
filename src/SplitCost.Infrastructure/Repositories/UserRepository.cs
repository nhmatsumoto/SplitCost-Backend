using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;

namespace SplitCost.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly SplitCostDbContext _context;
    
    public UserRepository(SplitCostDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }


    //public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    //{
    //    var entity = user.ToEntity();
    //    var result = await _context.Users.AddAsync(entity, cancellationToken);
    //    return result.Entity.ToDomain();
    //}
    //public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

    //    return entity?.ToDomain();
    //}
        
    //public void Update(User user)
    //{
    //    var entry = user.ToEntity();
    //    _context.Users.Update(entry);
    //}
    //public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken)
    //{
    //    return await _context.Users.AnyAsync(x => x.Username == username, cancellationToken);
    //}
    //public async Task<bool> ExistsByEmailAsync(string email , CancellationToken cancellationToken)
    //{
    //    return await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
    //}
    //public async Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken)
    //{
    //    return await _context.Users.AnyAsync(x => x.Id == userId, cancellationToken);
    //}
}
