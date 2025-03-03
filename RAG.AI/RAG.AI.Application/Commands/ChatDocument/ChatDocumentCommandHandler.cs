using Microsoft.Extensions.Options;
using OpenRouterSharp.Core.InfraServices;
using OpenRouterSharp.Core.Models;
using RAG.AI.Infrastructure.Configurations;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Infrastructure.ExternalServices;

namespace RAG.AI.Application.Commands.ChatDocument;
public class ChatDocumentCommandHandler : IRequestHandler<ChatDocumentCommand, string>
{
    private readonly IVectorSearchService _vectorSearchService;
    private readonly IChatService _chatService;
    private readonly IOpenRouterService _openRouterService;
    private readonly ILogger _logger;
    private readonly RAGConfig _config;

    public ChatDocumentCommandHandler(IVectorSearchService vectorSearchService, ILogger logger, IOptions<RAGConfig> options, IChatService chatService, IOpenRouterService openRouterService)
    {
        _vectorSearchService = vectorSearchService;
        _logger = logger;
        _config = options.Value;
        _chatService = chatService;
        _openRouterService = openRouterService;
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
            "Asking {Model} with {Resultcount} contexts. from these {BookCount} docs:",
            _config.ChatCompletionModel,
            resultcount,
            documents.Length
        );
        foreach (var doc in documents)
        {
            _logger.Information(" - {document}", doc);
        }

        //var response = await _chatService.AskRaggedQuestion(request.Query, [.. textChunks]);
        var prompt = new PropmtRequest()
        {
            Model = "deepseek/deepseek-r1:free",
            Prompt = request.Query.ToPrompt([.. textChunks])
        };
        var response = await _openRouterService.OpenRouterChatService.SendMessageAsync(prompt);

        _logger.Information("--------- Answer -------------");
        _logger.Information("{Answer}", response.Message);

        return response.Message;
    }
}

