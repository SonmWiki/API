using Application.Data;
using Application.Features.Articles.GetPendingRevisions;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.GetCategoriesTree;

public class GetCategoriesTreeQueryHandler
    (IApplicationDbContext dbContext) : IRequestHandler<GetCategoriesTreeQuery, ErrorOr<GetCategoriesTreeResponse>>
{
    public async Task<ErrorOr<GetCategoriesTreeResponse>> Handle(GetCategoriesTreeQuery request,
        CancellationToken token)
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
    }
}