using RAG.AI.Domain.Aggregates.ScheduledJobLogAggregate;

namespace RAG.AI.Domain.SeedWork.RepositoryInterfaces;

public interface IScheduledJobLogRepository : IRepository<ScheduledJobLog, Guid>
{
}

