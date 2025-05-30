using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(Guid userId);
    void Update(User user);
    Task<bool> ExistsAsync(Guid userId);
}