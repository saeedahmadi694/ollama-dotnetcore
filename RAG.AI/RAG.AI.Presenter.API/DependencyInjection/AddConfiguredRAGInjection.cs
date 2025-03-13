using Microsoft.SemanticKernel;
using RAG.AI.Infrastructure.Configurations;
using RAG.AI.Infrastructure.ExternalServices;

#pragma warning disable SKEXP0070

namespace RAG.AI.Presenter.API.DependencyInjection;

public static class AddConfiguredRAGInjection
{
    public static IServiceCollection AddConfiguredRAG(this IServiceCollection services, IConfiguration configuration)
    {

        var ragConfig = new RAGConfig();
        ragConfig = configuration.GetSection(RAGConfig.Key).Get<RAGConfig>();

        services.AddSingleton<IChatService, ChatService>();

        services.AddQdrantVectorStore();
        services.AddSingleton<IVectorSearchService, VectorSearchService>();

        HttpClient httpClient = new()
        {
            Timeout = TimeSpan.FromMinutes(5),
            BaseAddress = ragConfig.OllamaUrl
        };

        services.AddSingleton(serviceProvider =>
        {
            var kernelBuilder = Kernel.CreateBuilder();

            // Configure Ollama for both chat completion and embeddings
            kernelBuilder.AddOllamaChatCompletion(
                ragConfig.ChatCompletionModel,
                httpClient                
            );
            kernelBuilder.AddOllamaTextEmbeddingGeneration(
                ragConfig.EmbeddingModel,
                ragConfig.OllamaUrl
            );

            return kernelBuilder.Build();
        });

        return services;
    }
}