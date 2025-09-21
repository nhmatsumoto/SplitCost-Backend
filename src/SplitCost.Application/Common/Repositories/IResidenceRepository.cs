using SplitCost.Domain.Entities;

namespace SplitCost.Application.Common.Repositories;

public interface IResidenceRepository : IRepository<Residence>
{
    Task<bool> UserHasResidence(Guid userId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<Residence?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
