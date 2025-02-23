using RAG.AI.Domain.Aggregates.ScheduledJobLogAggregate;
using RAG.AI.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Transactions;

namespace RAG.AI.Infrastructure.Persistent;

public class AIContext : DbContext
{
    public const string DEFAULT_SCHEMA = "AI";
    //Here define DbSets<TEntites>
    public DbSet<ScheduledJobLog> ScheduledJobs { get; set; }

    private IDbContextTransaction _currentTransaction;

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public AIContext(DbContextOptions<AIContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly, t => t.GetInterfaces().Any(i =>
              i.IsGenericType &&
              i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync();

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }



}

public class AIReadOnlyContext : AIContext
{
    public AIReadOnlyContext(DbContextOptions<AIContext> options) : base(options)
    {
    }
}


