﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Microsoft.SemanticKernel.Embeddings;
using Qdrant.Client;
using RAG.AI.Infrastructure.Dtos.Common;
using RaggedBooks.Core.Configuration;
using System.Diagnostics;

namespace RAG.AI.Infrastructure.ExternalServices;



public class VectorSearchService : IVectorSearchService
{
    private readonly RAGConfig _raggedBookConfig;
    private readonly ILogger<VectorSearchService> _logger;
    private readonly ITextEmbeddingGenerationService _textEmbeddingGenerationService;
    private readonly IVectorStoreRecordCollection<ulong, ContentChunk> _collection;

    public VectorSearchService(
        Kernel kernel,
        RAGConfig raggedBookConfig,
        ILogger<VectorSearchService> logger
    )
    {
        _raggedBookConfig = raggedBookConfig;
        _logger = logger;
        _textEmbeddingGenerationService =
            kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        // note that we are skipping the portnumber as grpc port defaultss to 6334
        var vectorStore = new QdrantVectorStore(new QdrantClient(_raggedBookConfig.QdrantUrl.Host));
        var vectorStoreRecordDefinition = SetupVectorStoreRecordDefinition();

        _collection = vectorStore.GetCollection<ulong, ContentChunk>(
            _raggedBookConfig.VectorCollectionname,
            vectorStoreRecordDefinition
        );
    }

    private VectorStoreRecordDefinition SetupVectorStoreRecordDefinition()
    {
        var vectorStoreRecordDefinition = new VectorStoreRecordDefinition()
        {
            Properties = new List<VectorStoreRecordProperty>()
            {
                new VectorStoreRecordKeyProperty("Id", typeof(Guid)),
                new VectorStoreRecordDataProperty("Book", typeof(string)) { IsFilterable = true },
                new VectorStoreRecordDataProperty("BookFilename", typeof(string))
                {
                    IsFilterable = true,
                },
                new VectorStoreRecordDataProperty("Chapter", typeof(string)),
                new VectorStoreRecordDataProperty("PageNumber", typeof(int)),
                new VectorStoreRecordDataProperty("Index", typeof(int)),
                new VectorStoreRecordDataProperty("Content", typeof(string)),
                new VectorStoreRecordVectorProperty(
                    "ContentEmbedding",
                    typeof(ReadOnlyMemory<float>)
                )
                {
                    Dimensions = _raggedBookConfig.EmbeddingDimensions,
                },
            },
        };

        return vectorStoreRecordDefinition;
    }


    public async Task ClearCollection()
    {
        var collection = await GetCollection();
        await collection.DeleteCollectionAsync();
    }

    public async Task UpsertItems(ContentChunk[] items)
    {
        var collection = await GetCollection();
        var sw = Stopwatch.StartNew();
        var keys = new List<ulong>();
        await foreach (var key in collection.UpsertBatchAsync(items))
        {
            keys.Add(key);
        }

        _logger.LogInformation(
            "Added {RecordCount} records to Qdrant in {ElapsedMs}ms",
            keys.Count,
            sw.ElapsedMilliseconds
        );
    }

    public async Task<IVectorStoreRecordCollection<ulong, ContentChunk>> GetCollection()
    {
        _logger.LogInformation("Creating collection if not exists");

        await _collection.CreateCollectionIfNotExistsAsync();

        return _collection;
    }

    public async Task<VectorSearchResults<ContentChunk>> SearchVectorStore(string query)
    {
        var searchVector = await _textEmbeddingGenerationService.GenerateEmbeddingAsync(query);
        var collection = await GetCollection();

        var searchResult = await collection.VectorizedSearchAsync(
            searchVector,
            new VectorSearchOptions { Top = 10, IncludeVectors = false }
        );
        return searchResult;
    }
}

