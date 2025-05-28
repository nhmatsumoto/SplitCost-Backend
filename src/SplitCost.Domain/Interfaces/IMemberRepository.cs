using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Interfaces;

public interface IMemberRepository
{
    Task AddAsync(Member member);
    Task<Dictionary<Guid, string>> GetUsersByResidenceId(Guid residenceId);
}
