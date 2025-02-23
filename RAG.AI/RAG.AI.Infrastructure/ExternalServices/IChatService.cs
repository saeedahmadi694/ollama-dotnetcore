namespace RAG.AI.Infrastructure.ExternalServices;

public interface IChatService
{
    Task<string> AskRaggedQuestion(string question, string[] contexts);
}
