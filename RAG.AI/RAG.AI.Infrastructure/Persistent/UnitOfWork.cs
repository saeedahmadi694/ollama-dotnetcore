using Microsoft.EntityFrameworkCore.Storage;
using RAG.AI.Domain.SeedWork.RepositoryInterfaces;
using RAG.AI.Infrastructure.Persistent.Repositories;

namespace RAG.AI.Infrastructure.Persistent;

public class UnitOfWork : IUnitOfWork
{
    private readonly AIContext _dbContext;
    private readonly IMediator _mediator;

    public UnitOfWork(AIContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public bool HasActiveTransaction => _dbContext.HasActiveTransaction;

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _dbContext.Database.CreateExecutionStrategy();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _dbContext.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {

        await _dbContext.CommitTransactionAsync(transaction);
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }


    #region Repositories

    private IScheduledJobLogRepository _scheduledJobLogRepository;
    public IScheduledJobLogRepository ScheduledJobLogRepository
    {
        get
        {
            if (_scheduledJobLogRepository == null)
                _scheduledJobLogRepository = new ScheduledJobLogRepository(_dbContext);
            return _scheduledJobLogRepository;
        }
    }
    #endregion
}



