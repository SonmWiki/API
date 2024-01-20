using Application.Data;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Navigations.GetNavigationTree;

public class GetNavigationsTreeQueryHandler
    (IApplicationDbContext dbContext) : IRequestHandler<GetNavigationsTreeQuery, ErrorOr<GetNavigationsTreeResponse>>
{
    public async Task<ErrorOr<GetNavigationsTreeResponse>> Handle(GetNavigationsTreeQuery request,
        CancellationToken token)
    {
        var list = await dbContext.Navigations
            .Include(e => e.Children)
            .ToListAsync(token);

        var lookup = list.ToLookup(e => e.ParentId, e => new GetNavigationsTreeResponse.Element(
                e.Id,
                e.Weight,
                e.Name,
                e.Uri,
                new List<GetNavigationsTreeResponse.Element>()
            )
        );
        
        foreach (var node in lookup.SelectMany(e => e))
            node.Children.AddRange(lookup[node.Id]);
        
        var rootNodes = lookup[null].ToList();

        return new GetNavigationsTreeResponse(rootNodes);
    }
}