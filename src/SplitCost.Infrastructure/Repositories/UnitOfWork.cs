using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SplitCost.Application.Common.Interfaces;


namespace SplitCost.Infrastructure.Repositories;

public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed = false;

    public UnitOfWork(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
            throw new InvalidOperationException("Uma transação já está ativa.");

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public void BeginTransaction()
    {
        if (_transaction != null)
            throw new InvalidOperationException("Uma transação já está ativa.");

        _transaction = _context.Database.BeginTransaction();
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Nenhuma transação ativa para commitar.");

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public void Commit()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Nenhuma transação ativa para commitar.");

        try
        {
            _context.SaveChanges(); // Corrigido: garante persistência dos dados
            _transaction.Commit();
        }
        catch
        {
            Rollback();
            throw;
        }
        finally
        {
            DisposeTransaction();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await DisposeTransactionAsync();
        }
    }

    public void Rollback()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
            DisposeTransaction();
        }
    }

    private async Task<int> SaveChangesInternalAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    private int SaveChangesInternal()
    {
        return _context.SaveChanges();
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    private void DisposeTransaction()
    {
        _transaction?.Dispose();
        _transaction = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}