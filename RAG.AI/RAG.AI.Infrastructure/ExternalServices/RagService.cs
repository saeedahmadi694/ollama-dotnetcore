
namespace RAG.AI.Infrastructure.ExternalServices;



public class RagService : IRagService
{
    private readonly IChatService _chatService;
    private readonly IVectorSearchService _vectorSearchService;

    public RagService(IChatService chatService,IVectorSearchService vectorSearchService)
    {
        _chatService = chatService;
        _vectorSearchService = vectorSearchService;
    }


    public async Task<string> Search(string query)
    {
        
        var searchResult = await _vectorSearchService.SearchVectorStore(query);
        var searchResults = searchResult
            .Results.ToBlockingEnumerable()
            .Select(x => x)
            .ToList();

        var matchingRecords = searchResults.Select(x => x.Record).ToArray();
        var contexts = new List<string>();
        foreach (var record in matchingRecords)
        {
            var text =
                $"{record.Content}{Environment.NewLine}(source: {record.Document} - )";

            contexts.Add(text.Trim());
        }

        var response = await _chatService.AskRaggedQuestion(query, [.. contexts]);
        return response;
    }
}