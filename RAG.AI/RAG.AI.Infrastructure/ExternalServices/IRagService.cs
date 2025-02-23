

namespace RAG.AI.Infrastructure.ExternalServices;

public interface IRagService
{
    Task<string> Search(string query);
}