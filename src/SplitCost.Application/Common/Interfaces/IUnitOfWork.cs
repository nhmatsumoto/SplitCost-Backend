namespace SplitCost.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    //Async
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);

    //Sync
    void BeginTransaction();
    void Commit();
    void Rollback();
}
