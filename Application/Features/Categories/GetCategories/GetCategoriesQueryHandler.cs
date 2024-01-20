using Application.Data;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.GetCategories;

public class GetCategoriesQueryHandler
    (IApplicationDbContext dbContext) : IRequestHandler<GetCategoriesQuery, ErrorOr<GetCategoriesResponse>>
{
    public async Task<ErrorOr<GetCategoriesResponse>> Handle(GetCategoriesQuery request,
        CancellationToken token)
    {
        var list = await dbContext.Categories
            .Select(e => new GetCategoriesResponse.Element(e.Id, e.Name, e.ParentId))
            .AsNoTracking()
            .ToListAsync(token);

        return new GetCategoriesResponse(list);
    }
}