using Microsoft.Extensions.Options;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using RAG.AI.Infrastructure.Configurations;

namespace RAG.AI.Infrastructure.Persistent.DataSeeding;
public class DataSeeder
{
    private readonly RAGConfig _config;
    private readonly QdrantClient _qdrantClient;

    public DataSeeder(IOptions<RAGConfig> options, QdrantClient qdrantClient)
    {
        _config = options.Value;
        _qdrantClient = qdrantClient;
    }

    public async Task InitializeAsync()
    {
        await SetupCollectionAsync();
    }

    private async Task SetupCollectionAsync()
    {
        // Define vector parameters
        var vectorParams = new VectorParams
        {
            Size = 100,
            Distance = Distance.Cosine
        };

        // Check if the collection exists
        var collectionExists = await _qdrantClient.CollectionExistsAsync(_config.VectorCollectionName);

        if (!collectionExists)
        {
            // Create the collection if it doesn't exist
            await _qdrantClient.CreateCollectionAsync(_config.VectorCollectionName, vectorParams);
        }
        else
        {
            // Optionally, retrieve and log collection details
            var collectionInfo = await _qdrantClient.GetCollectionInfoAsync(_config.VectorCollectionName);
            Console.WriteLine($"Collection '{_config.VectorCollectionName}' already exists with status: {collectionInfo.Status}");
        }
    }



    public async Task ClearCollectionAsync()
    {
        await _qdrantClient.DeleteCollectionAsync(_config.VectorCollectionName);
    }
}
