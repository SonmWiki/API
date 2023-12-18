using Application.Data;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.GetCategories;

public class GetCategoriesQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetCategoriesQuery, ErrorOr<GetCategoriesResponse>>
{
    public async Task<ErrorOr<GetCategoriesResponse>> Handle(GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var list = await dbContext.Categories
            .Select(e => new GetCategoriesResponseElement(e.Id, e.Name, e.ParentId))
            .ToListAsync(cancellationToken: cancellationToken);

        return new GetCategoriesResponse(list);
    }
}