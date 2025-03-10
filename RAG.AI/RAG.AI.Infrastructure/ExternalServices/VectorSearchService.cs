using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Microsoft.SemanticKernel.Embeddings;
using Qdrant.Client.Grpc;
using RAG.AI.Domain.Aggregates.DocumentAggregate;
using RAG.AI.Infrastructure.Configurations;
using RAG.AI.Infrastructure.Dtos.Common;
using RAG.AI.Infrastructure.Extentions.Adapters.DocumentFiles;
using System.Diagnostics;
using static Qdrant.Client.Grpc.Conditions;
using QdrantClient = Qdrant.Client.QdrantClient;

namespace RAG.AI.Infrastructure.ExternalServices;


#pragma warning disable SKEXP0001

public class VectorSearchService : IVectorSearchService
{
    private readonly RAGConfig _config;
    private readonly ILogger<VectorSearchService> _logger;
    private readonly ITextEmbeddingGenerationService _textEmbeddingGenerationService;
    private readonly Qdrant.Client.QdrantClient _qdrantClient;

    public VectorSearchService(
        Kernel kernel,
        IOptions<RAGConfig> _optionsa,
        ILogger<VectorSearchService> logger
    )
    {
        _config = _optionsa.Value;
        _logger = logger;
        _textEmbeddingGenerationService =
            kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        _qdrantClient = new QdrantClient(_config.QdrantUrl.Host);
    }

    //private VectorStoreRecordDefinition SetupVectorStoreRecordDefinition()
    //{
    //    var vectorStoreRecordDefinition = new VectorStoreRecordDefinition()
    //    {
    //        Properties =
    //        [
    //            new VectorStoreRecordKeyProperty("Id", typeof(Guid)),
    //            new VectorStoreRecordDataProperty("DocumentId", typeof(string)){ IsFilterable = true },
    //            new VectorStoreRecordDataProperty("Document", typeof(string)) { IsFilterable = true },
    //            new VectorStoreRecordDataProperty("FileName", typeof(string)) { IsFilterable = true },
    //            new VectorStoreRecordDataProperty("PageNumber", typeof(int)),
    //            new VectorStoreRecordDataProperty("Index", typeof(int)),
    //            new VectorStoreRecordDataProperty("Content", typeof(string)),
    //            new VectorStoreRecordVectorProperty("ContentEmbedding",typeof(ReadOnlyMemory<float>)){Dimensions = _config.EmbeddingDimensions},
    //        ],
    //    };

    //    return vectorStoreRecordDefinition;
    //}


    public async Task ClearCollection()
    {
        await _qdrantClient.DeleteCollectionAsync(_config.VectorCollectionName);
    }

    public async Task UpsertItems(DocumentFile[] items)
    {
        var sw = Stopwatch.StartNew();
        var points = new List<PointStruct>();


        //public required string FileName { get; set; }
        //public required string DocumentId { get; set; }
        //public int PageNumber { get; set; }
        //public int Index { get; set; }
        //public required string Content { get; set; }

        foreach (var item in items)
        {
            points.Add(
                 new()
                 {
                     Id = item.Id,
                     Vectors = item.ContentEmbedding.ToArray(),
                     Payload = {
                        [nameof(DocumentFile.DocumentId)] = item.DocumentId,
                        [nameof(DocumentFile.FileName)] = item.FileName,
                        [nameof(DocumentFile.PageNumber)] = item.PageNumber,
                        [nameof(DocumentFile.Index)] = item.Index,
                        [nameof(DocumentFile.Content)] = item.Content
                     }
                 }
                );
        }


        await _qdrantClient.UpsertAsync(_config.VectorCollectionName, points);


        _logger.LogInformation(
            "Added {RecordCount} records to Qdrant in {ElapsedMs}ms",
            items.Length,
            sw.ElapsedMilliseconds
        );
    }

    public async Task<CollectionInfo> GetCollection()
    {
        _logger.LogInformation("Creating collection if not exists");
        return await _qdrantClient.GetCollectionInfoAsync(_config.VectorCollectionName);
    }

    public async Task<IReadOnlyList<ScoredPointDto>> SearchVectorStore(string query, List<string> docIds)
    {
        var searchVector = await _textEmbeddingGenerationService.GenerateEmbeddingAsync(query);
        //var collection = await GetCollection();
        var condition = Match("DocumentId", docIds);

        var points = await _qdrantClient.SearchAsync(
          _config.VectorCollectionName,
          searchVector,
          //condition,
          limit: 5);

        //var searchResult = await collection.VectorizedSearchAsync(
        //    searchVector,
        //    new VectorSearchOptions { Top = 10, IncludeVectors = false }
        //);


        return points.Select(r => r.ToDto()).ToList();
    }

}

