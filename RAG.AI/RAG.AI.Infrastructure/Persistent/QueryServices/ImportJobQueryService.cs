using RAG.AI.Domain.Aggregates.ImportAggregate;
using RAG.AI.Infrastructure.Persistent.QueryServices.QueryServiceInterfaces;
using System.Data;

namespace RAG.AI.Infrastructure.Persistent.QueryServices;
public class ImportJobQueryService : BaseQueryService<ImportJob, int>, IImportJobQueryService
{
    public ImportJobQueryService(AIReadOnlyContext dbContext, IDbConnectionFactory dbConnection) : base(dbContext, dbConnection)
    {
    }

}
