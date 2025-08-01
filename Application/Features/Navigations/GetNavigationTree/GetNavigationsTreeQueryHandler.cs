using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Navigations.GetNavigationTree;

public class GetNavigationsTreeQueryHandler(IApplicationDbContext dbContext) : IGetNavigationsTreeQueryHandler
{
    public async Task<ErrorOr<GetNavigationsTreeResponse>> Handle(GetNavigationsTreeQuery request,
        CancellationToken token)
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
                new List<GetNavigationsTreeResponse.Element>()
            )
        );
        
        foreach (var node in lookup.SelectMany(e => e))
            node.Children.AddRange(lookup[node.Id].OrderByDescending(e=>e.Weight));
        
        var rootNodes = lookup[null].OrderByDescending(e=>e.Weight).ToList();

        return new GetNavigationsTreeResponse(rootNodes);
    }
}