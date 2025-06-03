using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Entities;

namespace SplitCost.Infrastructure.Repositories;

public class ResidenceRepository : IResidenceRepository
{
    private readonly SplitCostDbContext _context;
    private readonly IMapper _mapper;

    public ResidenceRepository(SplitCostDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task AddAsync(Residence residence, CancellationToken cancellationToken)
    {
        var residenceEntity = _mapper.Map<ResidenceEntity>(residence);
        await _context.Residences.AddAsync(residenceEntity, cancellationToken);
    }

    public async Task<Residence?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var residenceEntity = await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Include(r => r.Expenses)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return _mapper.Map<Residence>(residenceEntity);
    }

    public async Task<Residence?> GetByUserIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var residenceEntity = await _context.Residences
            .Where(x => x.CreatedByUserId == id)
            .Include(x => x.Address)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return _mapper.Map<Residence>(residenceEntity); 
    }
    public void Update(Residence residence)
    {
        var residenceEntity = _mapper.Map<ResidenceEntity>(residence);
        _context.Residences.Update(residenceEntity);
    }
    public async Task<IEnumerable<Residence>> GetAllAsync(CancellationToken cancellationToken)
    {
        var residencesEntity = await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Include(r => r.Expenses)
                .ThenInclude(e => e.Shares)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<Residence>>(residencesEntity);
    }
    public async Task<bool> UserHasResidence(Guid userId, CancellationToken cancellationToken)
        => await _context.Residences
            .AnyAsync(r => r.Members
            .Any(m => m.UserId == userId), cancellationToken);
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) 
        => await _context.Residences
            .AnyAsync(r => r.Id == id, cancellationToken);
}
