using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;
using SplitCost.Infrastructure.Entities;

namespace SplitCost.Infrastructure.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly SplitCostDbContext _context;
    private readonly IMapper _mapper;

    public MemberRepository(SplitCostDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task AddAsync(Member member)
    {
        var memberEntity = _mapper.Map<MemberEntity>(member);
        await _context.Members.AddAsync(memberEntity);
    }
       
    // Obtem dicionario id, nome dos usuários que pertencem a uma residência
    // Usado para exibir os membros de uma residência no dropdown de despesas
    public async Task<Dictionary<Guid, string>> GetUsersByResidenceId(Guid residenceId)
        => await _context.Members
            .Where(rm => rm.ResidenceId == residenceId)
            .Select(rm => new { rm.UserId, rm.User.Name })
            .ToDictionaryAsync(x => x.UserId, x => x.Name);

    //verificar se membro existe em uma residência
    public async Task<bool> ExistsAsync(Guid userId, Guid residenceId)
        => await _context.Members
            .AnyAsync(m => m.UserId == userId && m.ResidenceId == residenceId);
}
