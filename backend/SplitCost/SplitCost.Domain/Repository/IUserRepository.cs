using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Repository;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(Guid userId);
    Task UpdateAsync(User user);
}