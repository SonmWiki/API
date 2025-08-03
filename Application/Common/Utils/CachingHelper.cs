using Application.Common.Caching;
using Application.Common.Messaging;

namespace Application.Common.Utils;

public static class CachingHelper
{
    public static async Task<TResponse> GetOrCacheAsync<TRequest, TResponse>(
        ICacheService cacheService,
        TRequest request,
        Func<Task<TResponse>> dataRetriever,
        CancellationToken token)
        where TRequest : ICachedQuery
    {
        if (request.IgnoreCaching)
            return await dataRetriever();

        return await cacheService.GetOrCreateAsync(
            request.Key,
            _ => dataRetriever(),
            request.Expiration,
            token);
    }
}