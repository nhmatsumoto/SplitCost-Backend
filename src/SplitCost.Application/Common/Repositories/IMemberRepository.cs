using SplitCost.Domain.Entities;

namespace SplitCost.Application.Common.Repositories;
public interface IMemberRepository
{
    Task<Member> AddAsync(Member member, CancellationToken cancellationToken);
    Task<Dictionary<Guid, string>> GetUsersByResidenceId(Guid residenceId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid userId, Guid residenceId, CancellationToken cancellationToken);
}
