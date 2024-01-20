using Application.Data;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.GetCategoryArticles;

public class GetCategoryArticlesQueryHandler
    (IApplicationDbContext dbContext) : IRequestHandler<GetCategoryArticlesQuery, ErrorOr<GetCategoryArticlesResponse>>
{
    public async Task<ErrorOr<GetCategoryArticlesResponse>> Handle(GetCategoryArticlesQuery request,
        CancellationToken token)
    {
        var category = await dbContext.Categories.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, token);

        if (category == null) return Errors.Category.NotFound;

        var articlesList = await dbContext.ArticleCategories
            .Where(e => e.CategoryId == request.Id)
            .Include(e => e.Article.CurrentRevision)
            .Select(e => e.Article)
            .Where(e => e.CurrentRevisionId != null && e.RedirectArticleId == null)
            .Select(e => new GetCategoryArticlesResponse.Element(e.Id, e.Title))
            .AsNoTracking()
            .ToListAsync(token);

        return new GetCategoryArticlesResponse(articlesList);
    }
}