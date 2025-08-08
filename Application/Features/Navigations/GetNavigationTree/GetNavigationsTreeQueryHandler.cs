using Application.Common.Caching;
using Application.Common.Messaging;
using Application.Common.Utils;
using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Navigations.GetNavigationTree;

public class GetNavigationsTreeQueryHandler(IApplicationDbContext dbContext, ICacheService cacheService)
    : IQueryHandler<GetNavigationsTreeQuery, GetNavigationsTreeResponse>
{
    public async Task<ErrorOr<GetNavigationsTreeResponse>> HandleAsync(GetNavigationsTreeQuery request,
        CancellationToken token)
    {
        return await CachingHelper.GetOrCacheAsync(cacheService, request, async () =>
        {
            var navigations = await dbContext.Navigations
                .Include(e => e.Children)
                .AsNoTracking()
                .ToListAsync(token);

            var lookup = navigations.ToLookup(e => e.ParentId, e => new GetNavigationsTreeResponse.Element(
                    e.Id,
                    e.Weight,
                    e.Name,
                    e.Uri,
                    e.Icon,
                    []
                )
            );

            foreach (var node in lookup.SelectMany(e => e))
                node.Children.AddRange(lookup[node.Id].OrderByDescending(e=>e.Weight));

            var rootNodes = lookup[null].OrderByDescending(e=>e.Weight).ToList();

            return new GetNavigationsTreeResponse(rootNodes);
        }, token);
    }
}