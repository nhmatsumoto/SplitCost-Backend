using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Interfaces;

public interface IMemberRepository
{
    Task AddAsync(Member member, CancellationToken cancellationToken);
    Task<Dictionary<Guid, string>> GetUsersByResidenceId(Guid residenceId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid userId, Guid residenceId, CancellationToken cancellationToken);
}
