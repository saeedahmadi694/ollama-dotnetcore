
using Microsoft.Extensions.VectorData;
using RAG.AI.Infrastructure.Dtos.Common;

namespace RAG.AI.Infrastructure.ExternalServices;

public interface IVectorSearchService
{
    Task<VectorSearchResults<ContentChunk>> SearchVectorStore(string query);
    Task ClearCollection();
    Task UpsertItems(ContentChunk[] items);
    Task<IVectorStoreRecordCollection<ulong, ContentChunk>> GetCollection();
}
