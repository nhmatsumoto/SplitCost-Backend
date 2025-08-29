using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.Common.Repositories;

public interface IUserRepository : IRepository<User>
{
    //Task<User> AddAsync(User user, CancellationToken cancellationToken);
    //Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    //void Update(User user);
    //Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken);
    //Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
    //Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken);
}