using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Interfaces;

public interface IResidenceRepository
{
    Task AddAsync(Residence residence);
    Task<Residence?> GetByIdAsync(Guid id);
    Task<Residence?> GetByUserIdAsync(Guid id);
    Task<IEnumerable<Residence>> GetAllAsync();
    Task UpdateAsync(Residence residence);

    Task<Boolean> UserHasResidence(Guid userId);
}
