using OpenRouterSharp.AspNetCore.DependencyInjection;
using RAG.AI.Infrastructure.Configurations;

namespace RAG.AI.Presenter.API.DependencyInjection;

public static class AddConfiguredOpenRouterSharpInjection
{
    public static IServiceCollection AddConfiguredOpenRouterSharp(this IServiceCollection services, IConfiguration configuration)
    {

        var config = new OpenRouterSharpConfig();
        config = configuration.GetSection(OpenRouterSharpConfig.Key).Get<OpenRouterSharpConfig>();

        services.AddOpenRouterService(option =>
        {
            option.ApiKey = config.ApiKey;
            option.BaseUrl = config.BaseUrl;
        });

        return services;
    }
}