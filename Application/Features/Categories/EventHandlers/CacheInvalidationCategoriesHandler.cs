using Application.Common.Caching;
using Application.Common.Constants;
using Application.Features.Categories.CreateCategory;
using Application.Features.Categories.DeleteCategory;
using MediatR;

namespace Application.Features.Categories.EventHandlers;

public class CacheInvalidationCategoriesHandler(ICacheService cacheService) :
    INotificationHandler<CategoryCreatedEvent>,
    INotificationHandler<CategoryDeletedEvent>
{
    public async Task Handle(CategoryCreatedEvent notification, CancellationToken token)
    {
        await cacheService.RemoveAsync(CachingKeys.Categories.CategoriesAll, token);
        await cacheService.RemoveAsync(CachingKeys.Categories.CategoriesTree, token);
    }

    public async Task Handle(CategoryDeletedEvent notification, CancellationToken token)
    {
        await cacheService.RemoveAsync(CachingKeys.Categories.CategoriesAll, token);
        await cacheService.RemoveAsync(CachingKeys.Categories.CategoriesTree, token);
        await cacheService.RemoveAsync(CachingKeys.Categories.CategoryArticlesById(notification.Id), token);
    }
}