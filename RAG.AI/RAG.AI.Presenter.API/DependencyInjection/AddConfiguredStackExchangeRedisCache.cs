using Microsoft.Extensions.Caching.Memory;
using RAG.AI.Domain.SeedWork.Utilities;
using RAG.AI.Infrastructure.Configurations;
using MemoryCache = RAG.AI.Infrastructure.Utilities.MemoryCache;

namespace RAG.AI.Presenter.API.DependencyInjection;

public static class RedisCacheInjection
{
    public static IServiceCollection AddConfiguredStackExchangeRedisCache(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        var config = configuration.GetSection(RedisCacheConfig.Key).Get<RedisCacheConfig>();
        services.AddStackExchangeRedis(instancePrefix: "RAG", config);

        return services;
    }

    public static IServiceCollection AddStackExchangeRedis(this IServiceCollection services, string instancePrefix, RedisCacheConfig config)
    {
        services.AddMemoryCache();
        services.AddScoped((Func<IServiceProvider, ICache>)((x) => new MemoryCache(x.GetRequiredService<IMemoryCache>(), instancePrefix)));
        return services;
    }
}



