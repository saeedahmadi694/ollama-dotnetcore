namespace RAG.AI.Infrastructure.Dtos.Common;

public record DocumentDto(
    int JobId,
    string Title,
    List<Page> Pages,
    string Filename,
    Guid DocumentId
);