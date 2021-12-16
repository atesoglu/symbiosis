using Application.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Services;

public class CacheServiceRedis : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public CacheServiceRedis(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<string> GetAsync(string key, CancellationToken cancellationToken)
    {
        return await _distributedCache.GetStringAsync(key, cancellationToken);
    }

    public async Task SaveAsync(string key, string value, TimeSpan timeToLive, CancellationToken cancellationToken)
    {
        await _distributedCache.SetStringAsync(key, value, new DistributedCacheEntryOptions {SlidingExpiration = timeToLive}, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}