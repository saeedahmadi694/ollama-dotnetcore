using RAG.AI.Domain.Aggregates.ImportAggregate;

namespace RAG.AI.Infrastructure.Persistent.QueryServices.QueryServiceInterfaces;
public interface IImportJobQueryService : IQueryService<ImportJob, int>
{
}
