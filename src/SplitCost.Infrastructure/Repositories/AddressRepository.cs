using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;
using SplitCost.Infrastructure.Entities;

namespace SplitCost.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly SplitCostDbContext _context;
    private readonly IMapper _mapper;

    public AddressRepository(SplitCostDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task AddAsync(Address addressDomain)
    {
        var addressEntity = _mapper.Map<AddressEntity>(addressDomain);
        await _context.Addresses.AddAsync(addressEntity);
    }
    public async Task<Address?> GetByIdAsync(Guid id)
    {
        var addressEntity = await _context.Addresses.FirstOrDefaultAsync(r => r.Id == id);
        var addressDomain = _mapper.Map<Address>(addressEntity);
        return addressDomain;
    }
    public async Task UpdateAsync(Address addressDomain)
    {
        var addressEntity = _mapper.Map<AddressEntity>(addressDomain);
        _context.Addresses.Update(addressEntity);
    }
}
