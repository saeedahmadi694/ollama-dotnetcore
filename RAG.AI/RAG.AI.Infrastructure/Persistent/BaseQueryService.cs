using System.Data;

namespace RAG.AI.Infrastructure.Persistent;

public class BaseQueryService<TEntity, TKey> : IQueryService<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : struct
{
    protected readonly AIReadOnlyContext _dbContext;
    protected readonly DbSet<TEntity> _dataSet;
    protected IQueryable<TEntity> _query;
    protected readonly IDbConnectionFactory _dbConnectionFactory;
    public BaseQueryService(AIReadOnlyContext dbContext, IDbConnectionFactory dbConnection)
    {
        _dbContext = dbContext ?? dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dataSet = dbContext.Set<TEntity>();
        _query = _dataSet.AsNoTracking().AsQueryable();
        _dbConnectionFactory = dbConnection;
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dataSet.CountAsync(filter);
    }

    public IQueryable<TEntity> Get()
    {
        return _dataSet.AsQueryable();
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, int skipCount = 0, int maxPageSize = 20)
    {
        return await _dataSet.Where(filter)
            .Skip(skipCount)
            .Take(maxPageSize)
            .ToListAsync();
    }

    public virtual async Task<TEntity?> GetAsync(TKey key)
    {
        return await _dataSet.FindAsync(key);
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dataSet.Where(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dataSet.AnyAsync(filter);
    }
}


