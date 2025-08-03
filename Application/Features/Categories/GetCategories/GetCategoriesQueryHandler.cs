using Application.Common.Caching;
using Application.Common.Utils;
using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.GetCategories;

public class GetCategoriesQueryHandler(IApplicationDbContext dbContext, ICacheService cacheService) : IGetCategoriesQueryHandler
{
    public async Task<ErrorOr<GetCategoriesResponse>> Handle(GetCategoriesQuery request, CancellationToken token)
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