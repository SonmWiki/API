using Application.Common.Caching;
using Application.Common.Constants;
using Application.Features.Articles.DeleteArticle;
using Application.Features.Articles.ReviewRevision;
using Application.Features.Articles.SetRedirect;
using MediatR;

namespace Application.Features.Articles.EventHandlers;

public class CacheInvalidationArticleHandler(ICacheService cacheService) : 
    INotificationHandler<ArticleDeletedEvent>,
    INotificationHandler<RevisionReviewedEvent>,
    INotificationHandler<RedirectSetEvent>,
    INotificationHandler<ArticleChangedRevisionEvent>
{
    public async Task Handle(ArticleDeletedEvent notification, CancellationToken token)
    {
        await cacheService.RemoveAsync(CachingKeys.Articles.ArticleById(notification.Id), token);
    }

    public async Task Handle(RevisionReviewedEvent notification, CancellationToken token)
    {
        await cacheService.RemoveAsync(CachingKeys.Articles.ArticleById(notification.ArticleId), token);
    }

    public async Task Handle(RedirectSetEvent notification, CancellationToken token)
    {
        await cacheService.RemoveAsync(CachingKeys.Articles.ArticleById(notification.ArticleId), token);
        await cacheService.RemoveAsync(CachingKeys.Articles.ArticleById(notification.RedirectId), token);
    }

    public async Task Handle(ArticleChangedRevisionEvent notification, CancellationToken token)
    {
        var difference = notification.PreviousRevisionCategoryIds.Except(notification.CurrentRevisionCategoryIds)
            .Concat(notification.CurrentRevisionCategoryIds.Except(notification.PreviousRevisionCategoryIds));
        foreach (var s in difference)
        {
            await cacheService.RemoveAsync(CachingKeys.Categories.CategoryArticlesById(s), token);
        }
    }
}