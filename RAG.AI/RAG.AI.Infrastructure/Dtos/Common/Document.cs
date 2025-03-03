namespace RAG.AI.Infrastructure.Dtos.Common;

public record Document(
    int JobId,
    string Title,
    List<Page> Pages,
    string Filename,
    Guid DocumentId
);