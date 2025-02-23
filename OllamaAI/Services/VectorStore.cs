using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

public interface IVectorStore
{
    Task StoreDocuments(List<string> chunks, float[][] embeddings);
    Task<List<Document>> SearchSimilarDocuments(float[] queryEmbedding, int k);
}

public class VectorStore : IVectorStore
{
    private readonly RagDbContext _context;
    private readonly IConfiguration _configuration;

    public VectorStore(RagDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        //InitializeDatabase().Wait();
    }

    private async Task InitializeDatabase()
    {
        // Enable vector extension
        await _context.Database.ExecuteSqlRawAsync("CREATE EXTENSION IF NOT EXISTS vector;");

        // Create vector index
        await _context.Database.ExecuteSqlRawAsync(@"
            CREATE INDEX IF NOT EXISTS documents_embedding_idx 
            ON documents 
            USING ivfflat (embedding vector_cosine_ops)
            WITH (lists = 100);
        ");
    }

    public async Task StoreDocuments(List<string> chunks, float[][] embeddings)
    {
        // Prepare batch parameters
        var parameters = new List<NpgsqlParameter>();
        var valuesList = new List<string>();

        for (int i = 0; i < chunks.Count; i++)
        {
            var contentParam = new NpgsqlParameter($"content_{i}", chunks[i]);
            var vectorParam = new NpgsqlParameter($"vector_{i}", NpgsqlTypes.NpgsqlDbType.Unknown)
            {
                Value = new Pgvector.Vector(embeddings[i])
            };
            var createdAtParam = new NpgsqlParameter($"created_at_{i}", DateTime.UtcNow);

            parameters.AddRange(new[] { contentParam, vectorParam, createdAtParam });
            valuesList.Add($"(@content_{i}, @vector_{i}::vector, @created_at_{i})");
        }

        var sql = $@"
            INSERT INTO documents (content, embedding, created_at)
            VALUES {string.Join(",", valuesList)}";

        await _context.Database.ExecuteSqlRawAsync(sql, parameters.ToArray());
    }

    public async Task<List<Document>> SearchSimilarDocuments(float[] queryEmbedding, int k)
    {
        var queryVector = string.Join(",", queryEmbedding);

        // Using raw SQL because EF Core doesn't support vector operations natively
        var documents = await _context.Documents
            .FromSqlRaw(@"
                SELECT 
                    id,
                    content,
                    title,
                    metadata,
                    embedding,
                    created_at,
                    1 - (embedding <=> {0}::vector) as similarity
                FROM documents
                ORDER BY embedding <=> {0}::vector
                LIMIT {1}",
                queryVector, k)
            .Select(d => new Document
            {
                Id = d.Id.ToString(),
                Content = d.Content,
                Title = d.Title,
                Metadata = d.Metadata,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync();

        return documents;
    }

    public async Task StoreDocumentWithMetadata(
        string content, 
        float[] embedding, 
        string title = null, 
        Dictionary<string, string> metadata = null)
    {
        var document = new DocumentEntity
        {
            Content = content,
            Embedding = new Pgvector.Vector(embedding),
            Title = title,
            Metadata = metadata ?? new Dictionary<string, string>(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();
    }
} 