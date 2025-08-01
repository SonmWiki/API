using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.GetCategoryArticles;

public class GetCategoryArticlesQueryHandler(IApplicationDbContext dbContext) : IGetCategoryArticlesQueryHandler
{
    public async Task<ErrorOr<GetCategoryArticlesResponse>> Handle(GetCategoryArticlesQuery request,
        CancellationToken token)
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
    }
}