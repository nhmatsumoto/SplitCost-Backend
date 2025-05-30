using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;
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

    public async Task AddAsync(Residence residence)
    {
        var residenceEntity = _mapper.Map<ResidenceEntity>(residence);
        await _context.Residences.AddAsync(residenceEntity);
    }

    public async Task<Residence?> GetByIdAsync(Guid id)
    {
        var residenceEntity = await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Include(r => r.Expenses)
                .ThenInclude(e => e.Shares)
            .FirstOrDefaultAsync(r => r.Id == id);

        return _mapper.Map<Residence>(residenceEntity);
    }

    public async Task<Residence?> GetByUserIdAsync(Guid id)
    {
        var residenceEntity = await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Where(r => r.Members.Any(m => m.UserId == id))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return _mapper.Map<Residence>(residenceEntity); 
    }
    public void UpdateAsync(Residence residence)
    {
        var residenceEntity = _mapper.Map<ResidenceEntity>(residence);
        _context.Residences.Update(residenceEntity);
    }
    public async Task<IEnumerable<Residence>> GetAllAsync()
    {
        var residencesEntity = await _context.Residences
            .Include(r => r.Members)
                .ThenInclude(rm => rm.User)
            .Include(r => r.Expenses)
                .ThenInclude(e => e.Shares)
            .ToListAsync();

        return _mapper.Map<IEnumerable<Residence>>(residencesEntity);
    }
    public async Task<bool> UserHasResidence(Guid userId)
        => await _context.Residences
            .AnyAsync(r => r.Members
            .Any(m => m.UserId == userId));
    public async Task<bool> ExistsAsync(Guid id) 
        => await _context.Residences
            .AnyAsync(r => r.Id == id);
}
