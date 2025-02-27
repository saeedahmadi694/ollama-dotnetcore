using Microsoft.Extensions.Options;
using RAG.AI.Infrastructure.Configurations;
using RAG.AI.Infrastructure.ExternalServices;

namespace RAG.AI.Application.Commands.ChatDocument;
public class ChatDocumentCommandHandler : IRequestHandler<ChatDocumentCommand, string>
{
    private readonly IVectorSearchService _vectorSearchService;
    private readonly IChatService _chatService;
    private readonly ILogger _logger;
    private readonly RAGConfig _config;

    public ChatDocumentCommandHandler(IVectorSearchService vectorSearchService, ILogger logger, IOptions<RAGConfig> options, IChatService chatService)
    {
        _vectorSearchService = vectorSearchService;
        _logger = logger;
        _config = options.Value;
        _chatService = chatService;
    }

    public async Task<string> Handle(ChatDocumentCommand request, CancellationToken cancellationToken)
    {
        var resultcount = 5;

        var searchResult = await _vectorSearchService.SearchVectorStore(request.Query);
        var searchResults = searchResult.Results.ToBlockingEnumerable().Select(x => x).ToList();
        if (searchResults.Count == 0)
        {
            Console.WriteLine("No results");
            return "No results";
        }

        var textChunks = searchResults.Select(x => x.Record.Content).ToArray();
        var documents = searchResults.Select(x => x.Record.Document).Distinct().ToArray();
        _logger.Information(
            "Asking {Model} with {Resultcount} contexts. from these {BookCount} books:",
            _config.ChatCompletionModel,
            resultcount,
            documents.Length
        );
        foreach (var book in documents)
        {
            _logger.Information(" - {Book}", book);
        }

        var response = await _chatService.AskRaggedQuestion(request.Query, [.. textChunks]);

        _logger.Information("--------- Answer -------------");
        _logger.Information("{Answer}", response);

        return response;
    }
}

