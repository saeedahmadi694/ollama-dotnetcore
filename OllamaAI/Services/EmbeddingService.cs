public interface IEmbeddingService
{
    Task<float[]> GenerateEmbedding(string text);
}

