using Application.Common.Caching;
using Application.Common.Messaging;
using Application.Common.Utils;
using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.GetCategoryArticles;

public class GetCategoryArticlesQueryHandler(IApplicationDbContext dbContext, ICacheService cacheService)
    : IQueryHandler<GetCategoryArticlesQuery, GetCategoryArticlesResponse>
{
    public async Task<ErrorOr<GetCategoryArticlesResponse>> HandleAsync(GetCategoryArticlesQuery request,
        CancellationToken token)
    {
        return await CachingHelper.GetOrCacheAsync<GetCategoryArticlesQuery, ErrorOr<GetCategoryArticlesResponse>>(cacheService, request, async () =>
        {
            var category = await dbContext.Categories.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Id, token);

            if (category == null) return Errors.Category.NotFound;

            var articlesList = await dbContext.Articles
                .Where(e => e.RedirectArticleId == null && e.CurrentRevision.Categories.Any(x => x.Id == request.Id))
                .Select(e=> new GetCategoryArticlesResponse.Element(e.Id, e.Title))
                .AsNoTracking()
                .ToListAsync(token);

            return new GetCategoryArticlesResponse(articlesList);
        }, token);
    }
}