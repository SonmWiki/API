using Application.Common.Caching;
using Application.Common.Constants;
using Application.Features.Navigations.UpdateNavigationsTree;

namespace Application.Features.Navigations.EventHandlers;

//TODO: Reimplement without MediatR
// public class CacheInvalidationNavigationsHandler(ICacheService cacheService) :
//     INotificationHandler<NavigationsTreeUpdatedEvent>
// {
//     public async Task Handle(NavigationsTreeUpdatedEvent notification, CancellationToken token)
//     {
//         await cacheService.RemoveAsync(CachingKeys.Navigation.NavigationsTree, token);
//     }
// }