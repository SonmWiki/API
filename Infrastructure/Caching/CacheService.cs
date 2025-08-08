using Application.Common.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching;

public class CacheService(IMemoryCache memoryCache) : ICacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

    public async Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null, CancellationToken token = default)
    {
        var result = await memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(expiration ?? DefaultExpiration);
                return factory(token);
            });

        return result!;
    }

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}