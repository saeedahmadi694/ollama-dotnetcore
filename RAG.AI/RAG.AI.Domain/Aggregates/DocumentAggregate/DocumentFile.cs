namespace RAG.AI.Domain.Aggregates.DocumentAggregate;

public class DocumentFile
{
    public Guid Id { get; set; }
    public required string FileName { get; set; }
    public required string DocumentId { get; set; }
    public int PageNumber { get; set; }
    public int Index { get; set; }
    public required string Content { get; set; }
    public ReadOnlyMemory<float> ContentEmbedding { get; set; }
}
