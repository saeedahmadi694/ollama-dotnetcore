namespace RAG.AI.Infrastructure.Dtos.Common;

public record ScoredPointDto(
    string Id,
    double Score,
    long Version,
    int Index,
    int PageNumber,
    string DocumentId,
    string FileName,
    string Content
);


