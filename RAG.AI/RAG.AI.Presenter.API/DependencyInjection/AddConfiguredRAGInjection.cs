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
        //services.AddSingleton<IRagService, RagService>();

        // vector store and api
        services.AddQdrantVectorStore();
        services.AddSingleton<IVectorSearchService, VectorSearchService>();
        //services.AddSingleton<IConvertToBook, PdfToBookConverter>();
        //services.AddSingleton<QDrantApiClient>();

        //services.AddSingleton<FileImportService>();
        services.AddSingleton(serviceProvider =>
        {
            var kernelBuilder = Kernel.CreateBuilder();

            // Configure Ollama for both chat completion and embeddings
            kernelBuilder.AddOllamaChatCompletion(
                ragConfig.ChatCompletionModel,
                ragConfig.OllamaUrl
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