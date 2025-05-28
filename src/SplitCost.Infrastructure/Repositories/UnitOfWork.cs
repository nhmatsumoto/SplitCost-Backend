using SpitCost.Infrastructure.Context;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SplitCostDbContext _context;

    public UnitOfWork(SplitCostDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose() 
        => _context.Dispose();
}

