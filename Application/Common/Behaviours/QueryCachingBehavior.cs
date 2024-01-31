using Application.Common.Caching;
using Application.Common.Messaging;
using MediatR;

namespace Application.Common.Behaviours;

public class QueryCachingBehavior<TRequest, TResponse>(ICacheService cacheService) : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : ICachedQuery
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken token)
    {
        return await cacheService.GetOrCreateAsync(request.Key, _ => next(), request.Expiration, token);
    }
}