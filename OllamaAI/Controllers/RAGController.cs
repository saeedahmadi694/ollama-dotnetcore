using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

[ApiController]
[Route("api/[controller]")]
public class RAGController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorStore _vectorStore;
    private readonly ILLMService _llmService;

    public RAGController(
        IDocumentService documentService,
        IEmbeddingService embeddingService,
        IVectorStore vectorStore,
        ILLMService llmService)
    {
        _documentService = documentService;
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
        _llmService = llmService;
    }

    [HttpPost("query")]
    public async Task<IActionResult> Query([FromBody] QueryRequest request)
    {
        try
        {
            // 1. Generate embedding for the query
            var queryEmbedding = await _embeddingService.GenerateEmbedding(request.Query);

            // 2. Retrieve relevant documents
            var relevantDocs = await _vectorStore.SearchSimilarDocuments(queryEmbedding, k: 3);

            // 3. Create context from retrieved documents
            var context = _documentService.CreateContext(relevantDocs);

            // 4. Generate response using LLM with context
            var response = await _llmService.GenerateResponse(request.Query, context);

            return Ok(new QueryResponse
            {
                Answer = response,
                RelevantDocuments = relevantDocs
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("document")]
    public async Task<IActionResult> AddDocument([FromBody] AddDocumentRequest request)
    {
        try
        {
            // 1. Process and chunk the document
            var chunks = _documentService.ChunkDocument(request.Content);

            // 2. Generate embeddings for chunks
            var embeddings = await Task.WhenAll(
                chunks.Select(chunk => _embeddingService.GenerateEmbedding(chunk))
            );

            // 3. Store chunks and embeddings
            await _vectorStore.StoreDocuments(chunks, embeddings);

            return Ok(new { message = "Document processed and stored successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

// Request/Response Models
public class QueryRequest
{
    public string Query { get; set; }
}

public class QueryResponse
{
    public string Answer { get; set; }
    public List<Document> RelevantDocuments { get; set; }
}

public class AddDocumentRequest
{
    public string Content { get; set; }
    public string Title { get; set; }
} 