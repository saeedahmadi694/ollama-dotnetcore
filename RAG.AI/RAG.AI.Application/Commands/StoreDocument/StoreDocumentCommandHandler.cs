using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Text;
using RAG.AI.Domain.Aggregates.DocumentAggregate;
using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.Configurations;
using RAG.AI.Infrastructure.Dtos.Common;
using RAG.AI.Infrastructure.Exceptions.BaseExceptions;
using RAG.AI.Infrastructure.ExternalServices;


#pragma warning disable SKEXP0050
#pragma warning disable SKEXP0001

namespace RAG.AI.Application.Commands.StoreDocument;
public class StoreDocumentCommandHandler : IRequestHandler<StoreDocumentCommand, Unit>
{
    private readonly ILogger _logger;
    private readonly RAGConfig _config;
    private readonly IVectorSearchService _vectorSearchService;
    private readonly ITextEmbeddingGenerationService _textEmbeddingGenerationService;
    private readonly IUnitOfWork _unitOfWork;
    public StoreDocumentCommandHandler(ILogger logger, IOptions<RAGConfig> config, Kernel kernel, IVectorSearchService vectorSearchService, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _config = config.Value;
        _vectorSearchService = vectorSearchService;
        _textEmbeddingGenerationService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(StoreDocumentCommand request, CancellationToken cancellationToken)
    {
        var importJob = await _unitOfWork.ImportJobRepository.GetAsync(request.Doc.JobId);
        if (importJob is null)
            throw new NotFoundException("can not find importJob");

        var pages = request.Doc.Pages;
        var chunks = new List<DocumentFile>();

        _logger.Information(
            "Found {PageCount} pages in {BookTitle} {FileName}. Creating embeddings...",
            pages.Count,
            request.Doc.Title,
            request.Doc.Filename
        );
        int bookIndex = 0;
        foreach (var page in pages)
        {
            var paragraphs = TextChunker.SplitPlainTextParagraphs(
                TextChunker.SplitPlainTextLines(page.TextContent, _config.MaxTokensPerLine),
                _config.MaxTokensPerParagraph,
                _config.OverlapTokens
            );

            // cleanup linebreaks in paragraphs
            paragraphs = paragraphs.Select(x => x.Replace("-\n", " ")).ToList();

            var embeddings = await _textEmbeddingGenerationService.GenerateEmbeddingsAsync(
                paragraphs
            );

            foreach (var (index, paragraph) in paragraphs.Select((x, index) => (index, x)))
            {
                var embedding = embeddings[index];
                var chunk = new DocumentFile
                {
                    Id = Guid.NewGuid(),
                    DocumentId = request.Doc.DocumentId.ToString(),
                    PageNumber = page.Pagenumber,
                    Content = paragraph,
                    ContentEmbedding = embedding,
                    Index = bookIndex,
                    FileName = request.Doc.Filename,
                };
                chunks.Add(chunk);
                bookIndex++;
            }
        }
        //_logger.Information(
        //    "Created {ChunkCount} embeddings in {ElapsedMilliseconds}ms, averaging {AvgMs}ms per embedding",
        //    chunks.Count,
        //    sw.ElapsedMilliseconds,
        //    sw.ElapsedMilliseconds / chunks.Count
        //);

        await _vectorSearchService.UpsertItems(chunks.ToArray());
        importJob.SetAsSucceeded();
        return Unit.Value;
    }

}

