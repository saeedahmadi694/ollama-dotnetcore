using RAG.AI.Domain.Aggregates.ScheduledJobLogAggregate;
using RAG.AI.Domain.SeedWork.RepositoryInterfaces;

namespace RAG.AI.Infrastructure.Persistent.Repositories;

public class ScheduledJobLogRepository : BaseRepository<ScheduledJobLog, Guid>, IScheduledJobLogRepository
{
    public ScheduledJobLogRepository(AIContext dbContext) : base(dbContext)
    {
    }
}

