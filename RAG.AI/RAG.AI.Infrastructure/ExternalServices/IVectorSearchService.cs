
using Microsoft.Extensions.VectorData;
using Qdrant.Client.Grpc;
using RAG.AI.Domain.Aggregates.DocumentAggregate;
using RAG.AI.Infrastructure.Dtos.Common;

namespace RAG.AI.Infrastructure.ExternalServices;

public interface IVectorSearchService
{
    Task<IReadOnlyList<ScoredPointDto>> SearchVectorStore(string query, List<string> docId);
    Task ClearCollection();
    Task UpsertItems(DocumentFile[] items);
    Task<IVectorStoreRecordCollection<ulong, DocumentFile>> GetCollection();
}
