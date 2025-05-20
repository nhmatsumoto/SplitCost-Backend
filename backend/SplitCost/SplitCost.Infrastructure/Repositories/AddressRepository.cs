using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly SplitCostDbContext _context;

    public AddressRepository(SplitCostDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(Address address)
    {
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
    }

    public async Task<Address?> GetByIdAsync(Guid id) =>
        await _context.Addresses.FirstOrDefaultAsync(r => r.Id == id);
   

    public async Task UpdateAsync(Address address)
    {
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
    }
}
