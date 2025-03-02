using Microsoft.EntityFrameworkCore.Storage;
using RAG.AI.Domain.SeedWork.RepositoryInterfaces;

namespace RAG.AI.Domain.SeedWork;

public interface IUnitOfWork
{

    #region Repositories

    IScheduledJobLogRepository ScheduledJobLogRepository { get; }
    IImportJobRepository ImportJobRepository { get; }


    #endregion


    bool HasActiveTransaction { get; }
    IExecutionStrategy CreateExecutionStrategy();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync(IDbContextTransaction transaction);
    void SaveChanges();
    Task SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}


