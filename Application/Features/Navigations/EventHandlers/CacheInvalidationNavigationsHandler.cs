using Application.Common.Caching;
using Application.Common.Constants;
using Application.Features.Navigations.UpdateNavigationsTree;
using MediatR;

namespace Application.Features.Navigations.EventHandlers;

public class CacheInvalidationNavigationsHandler(ICacheService cacheService) :
    INotificationHandler<NavigationsTreeUpdatedEvent>
{
    public async Task Handle(NavigationsTreeUpdatedEvent notification, CancellationToken token)
    {
        await cacheService.RemoveAsync(CachingKeys.Navigation.NavigationsTree, token);
    }
}