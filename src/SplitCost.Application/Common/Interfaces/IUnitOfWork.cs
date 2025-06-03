namespace SplitCost.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    //Async
    Task BeginTransactionAsync();
    Task<int> SaveChangesAsync();
    Task CommitAsync();
    Task RollbackAsync();

    //Sync
    void BeginTransaction();
    int SaveChanges();
    void Commit();
    void Rollback();
    
}
