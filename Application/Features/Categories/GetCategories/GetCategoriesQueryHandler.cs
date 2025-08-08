using Application.Common.Caching;
using Application.Common.Messaging;
using Application.Common.Utils;
using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.GetCategories;

public class GetCategoriesQueryHandler(IApplicationDbContext dbContext, ICacheService cacheService)
    : IQueryHandler<GetCategoriesQuery, GetCategoriesResponse>
{
    public async Task<ErrorOr<GetCategoriesResponse>> HandleAsync(GetCategoriesQuery request, CancellationToken token)
    {
        return await CachingHelper.GetOrCacheAsync(cacheService, request, async () =>
        {
            var list = await dbContext.Categories
                .Select(e => new GetCategoriesResponse.Element(e.Id, e.Name, e.ParentId))
                .AsNoTracking()
                .ToListAsync(token);

            return new GetCategoriesResponse(list);
        }, token);
    }
}