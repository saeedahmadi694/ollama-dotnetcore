namespace RAG.AI.Infrastructure.Configurations;

public class RAGConfig
{
    public const string Key = "RAG";
    public Uri OllamaUrl { get; set; } = null!;
    public Uri QdrantUrl { get; set; } = null!;
    public string EmbeddingModel { get; set; } = string.Empty;
    public int EmbeddingDimensions { get; set; } = 1024;
    public string VectorCollectionName { get; set; } = "DocumentContent";
    public int MaxTokensPerLine { get; set; } = 300;
    public int MaxTokensPerParagraph { get; set; } = 512;
    public int OverlapTokens { get; set; } = 100;
    public string ChatCompletionModel { get; set; } = null!;
    public bool UseLocalChatModel { get; set; }
}
