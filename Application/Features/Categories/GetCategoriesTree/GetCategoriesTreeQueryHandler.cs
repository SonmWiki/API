using Application.Common.Caching;
using Application.Common.Utils;
using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.GetCategoriesTree;

public class GetCategoriesTreeQueryHandler(IApplicationDbContext dbContext, ICacheService cacheService) : IGetCategoriesTreeQueryHandler
{
    public async Task<ErrorOr<GetCategoriesTreeResponse>> Handle(GetCategoriesTreeQuery request, CancellationToken token)
    {
        return await CachingHelper.GetOrCacheAsync(cacheService, request, async () =>
        {
            var list = await dbContext.Categories
                .Include(e => e.SubCategories)
                .ToListAsync(token);

            var lookup = list.ToLookup(e => e.ParentId, e => new GetCategoriesTreeResponse.Element(
                    e.Id,
                    e.Name,
                    new List<GetCategoriesTreeResponse.Element>()
                )
            );

            foreach (var node in lookup.SelectMany(e => e))
                node.Children.AddRange(lookup[node.Id]);

            var rootNodes = lookup[null].ToList();

            return new GetCategoriesTreeResponse(rootNodes);
        }, token);
    }
}