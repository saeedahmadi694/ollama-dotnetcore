using RAG.AI.Domain.Aggregates.ImportAggregate;
using RAG.AI.Domain.SeedWork.RepositoryInterfaces;

namespace RAG.AI.Infrastructure.Persistent.Repositories;
public class ImportJobRepository : BaseRepository<ImportJob, int>, IImportJobRepository
{
    public ImportJobRepository(AIContext dbContext) : base(dbContext)
    {
    }

    public override async Task<ImportJob?> GetAsync(int key)
    {
        return await _dataSet
            .FirstAsync(e => e.Id == key);
    }

    public override async Task<ImportJob?> GetAsync(Expression<Func<ImportJob, bool>> filter)
    {
        return await _dataSet
            .FirstOrDefaultAsync(filter);
    }
}
