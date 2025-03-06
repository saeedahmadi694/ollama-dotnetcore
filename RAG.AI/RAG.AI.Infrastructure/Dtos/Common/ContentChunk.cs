namespace RAG.AI.Infrastructure.Dtos.Common;

public sealed class ContentChunk
{
    public Guid Id { get; set; }
    public required string Document { get; set; }
    public required string FileName { get; set; }
    public required string DocumentId { get; set; }
    public int PageNumber { get; set; }
    public int Index { get; set; }
    public required string Content { get; set; }
    public ReadOnlyMemory<float> ContentEmbedding { get; set; }
}
