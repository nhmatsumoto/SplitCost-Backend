using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;
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
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        await _context.Users.AddAsync(userEntity, cancellationToken);
    }
    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return _mapper.Map<User>(userEntity);
    }
        
    public void Update(User user)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        _context.Users.Update(userEntity);
    }
    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(x => x.Username == username, cancellationToken);
    }
    public async Task<bool> ExistsByEmailAsync(string email , CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
    }
    public async Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(x => x.Id == userId, cancellationToken);
    }
}
