using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Persistence.Mapping;

namespace SplitCost.Infrastructure.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly SplitCostDbContext _context;

    public MemberRepository(SplitCostDbContext context, IMapper mapper)
    {
        _context    = context   ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Member> AddAsync(Member member, CancellationToken cancellationToken)
    {
        var entity = member.ToEntity();
        var result = await _context.Members.AddAsync(entity, cancellationToken);
        return result.Entity.ToDomain();
    }
    public async Task<Dictionary<Guid, string>> GetUsersByResidenceId(Guid residenceId, CancellationToken cancellationToken)
    {
        return await _context.Members
            .Where(rm => rm.ResidenceId == residenceId)
            .Select(rm => new { rm.UserId, rm.User.Name })
            .ToDictionaryAsync(x => x.UserId, x => x.Name, cancellationToken);
    }
    public async Task<bool> ExistsAsync(Guid userId, Guid residenceId, CancellationToken cancellationToken)
        => await _context.Members
            .AnyAsync(m => m.UserId == userId && m.ResidenceId == residenceId, cancellationToken);
}
