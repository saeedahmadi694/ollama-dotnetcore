using Microsoft.Extensions.Caching.Memory;
using RAG.AI.Domain.SeedWork.Utilities;

namespace RAG.AI.Infrastructure.Utilities;

public class MemoryCache : ICache
{

    private readonly string Prefix;
    private readonly IMemoryCache _cache;

    public MemoryCache(IMemoryCache memoryCache, string instancePrefix)
    {
        _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));

        if (string.IsNullOrWhiteSpace(instancePrefix))
        {
            throw new ArgumentException("Invalid instance prefix. Provide a valid string as prefix");
        }

        Prefix = instancePrefix.Trim().Replace(" ", "") ?? "";
    }

    public async Task<T> GetAsync<T>(string key, bool hasAbsoluteKey = false)
    {
        var cacheKey = hasAbsoluteKey ? key : $"{Prefix}_{key}";
        return await Task.FromResult(_cache.TryGetValue(cacheKey, out T value) ? value : default);
    }

    public async Task<bool> SetAsync<T>(string key, T value)
    {
        var cacheKey = $"{Prefix}_{key}";
        _cache.Set(cacheKey, value);
        return await Task.FromResult(true);
    }

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan ttl)
    {
        var cacheKey = $"{Prefix}_{key}";
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        };
        _cache.Set(cacheKey, value, cacheEntryOptions);
        return await Task.FromResult(true);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        var cacheKey = $"{Prefix}_{key}";
        _cache.Remove(cacheKey);
        return await Task.FromResult(true);
    }
}



