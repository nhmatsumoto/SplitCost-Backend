using SplitCost.Domain.Entities;

namespace SplitCost.Application.Common.Repositories;

public interface IResidenceRepository
{
    Task AddAsync(Residence residence, CancellationToken cancellationToken);
    Task<Residence?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Residence?> GetByUserIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Residence>> GetAllAsync(CancellationToken cancellationToken);
    void Update(Residence residence);
    Task<Boolean> UserHasResidence(Guid userId, CancellationToken ctcancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
