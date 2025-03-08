using Qdrant.Client.Grpc;
using RAG.AI.Domain.Aggregates.DocumentAggregate;
using RAG.AI.Infrastructure.Dtos.Common;

namespace RAG.AI.Infrastructure.Extentions.Adapters.DocumentFiles;

public static class DocumentFileAdapter
{
    public static ScoredPointDto ToDto(this ScoredPoint point)
    {
        var index = point.Payload[nameof(DocumentFile.Index)].ToString().ToPayloadValue<int>();
        var pageNumber = point.Payload[nameof(DocumentFile.PageNumber)].ToString().ToPayloadValue<int>(); ;
        var documentId = point.Payload[nameof(DocumentFile.DocumentId)].ToString().ToPayloadValue<string>();
        var fileName = point.Payload["DocumentFileName"].ToString().ToPayloadValue<string>();
        //var fileName = point.Payload[nameof(DocumentFile.FileName)].ToString().ToPayloadValue<string>();
        var content = point.Payload[nameof(DocumentFile.Content)].ToString().ToPayloadValue<string>();
        return new ScoredPointDto(point.Id.Uuid, point.Score, (long)point.Version, index, pageNumber, documentId, fileName, content);
    }
}




