namespace RAG.AI.Infrastructure.Dtos.Common;

public record Document(
    string Title,
    List<Page> Pages,
    string Filename,
    Guid DocumentId
);