using Application.Services.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services.Cache;

public class CacheServiceInMemory : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public CacheServiceInMemory(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<string> GetAsync(string key, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_memoryCache.Get<string>(key));
    }

    public async Task SaveAsync(string key, string value, TimeSpan timeToLive, CancellationToken cancellationToken)
    {
        await Task.Run(() => _memoryCache.Set(key, value, new MemoryCacheEntryOptions {SlidingExpiration = timeToLive}), cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await Task.Run(() => _memoryCache.Remove(key), cancellationToken);
    }
}