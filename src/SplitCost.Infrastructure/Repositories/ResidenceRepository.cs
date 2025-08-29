using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;

namespace SplitCost.Infrastructure.Repositories;

public class ResidenceRepository : Repository<Residence>, IResidenceRepository
{
    private readonly SplitCostDbContext _context;

    public ResidenceRepository(SplitCostDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    //public async Task<Residence> AddAsync(Residence residence, CancellationToken cancellationToken)
    //{
    //    var entity = residence.ToEntity();
    //    var result = await _context.Residences.AddAsync(entity);
    //    return result.Entity.ToDomain();
    //}

    //public async Task<ResidenceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    //{
    //    return await _context.Residences
    //        .Where(r => r.Id == id)
    //        .Select(r => new ResidenceDto
    //        {
    //            Id = r.Id,
    //            Name = r.Name,
    //            Street = r.Street,
    //            Number = r.Number,
    //            Apartment = r.Apartment,
    //            City = r.City,
    //            Prefecture = r.Prefecture,
    //            Country = r.Country,
    //            PostalCode = r.PostalCode,
    //            CreatedByUserId = r.CreatedByUserId,
    //            Members = r.Members.Select(m => new MemberDto
    //            {
    //                Id = m.Id,
    //                UserId = m.UserId,
    //                JoinedAt = m.JoinedAt,
    //                MemberName = m.User.Name
    //            }).ToList(),
    //            Expenses = r.Expenses.Select(e => new ExpenseDto
    //            {
    //                Id = e.Id,
    //                Amount = e.Amount,
    //                Category = e.Category,
    //                Type = e.Type,
    //                Date = e.Date,
    //                Description = e.Description,
    //                RegisteredByUserId = e.RegisteredByUserId,
    //                PaidByUserId = e.PaidByUserId
    //            }).ToList()
    //        })
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync(cancellationToken);
    //}

    //public async Task<Residence?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.Residences
    //        .Where(x => x.CreatedByUserId == userId)
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync(cancellationToken);

    //    return entity?.ToDomain();
    //}

    //public void Update(Residence residence)
    //{
    //    var entry = residence.ToEntity();
    //    _context.Residences.Update(entry);
    //}

    //public async Task<IEnumerable<Residence>> GetAllAsync(CancellationToken cancellationToken)
    //{

    //    var entity = await _context.Residences
    //        .Include(r => r.Members)
    //            .ThenInclude(rm => rm.User)
    //        .Include(r => r.Expenses)
    //        .ToListAsync(cancellationToken);

    //    return entity.Select(e => e.ToDomain());
    //}

    //public async Task<bool> UserHasResidence(Guid userId, CancellationToken cancellationToken)
    //{
    //    return await _context.Residences
    //        .AnyAsync(r => r.Members.Any(m => m.UserId == userId), cancellationToken);
    //}

    //public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    //{
    //    return await _context.Residences
    //        .AnyAsync(r => r.Id == id, cancellationToken);
    //}
}
