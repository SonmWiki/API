using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.GetCategoryArticles;

public class GetCategoryArticlesQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetCategoryArticlesQuery, ErrorOr<GetCategoryArticlesResponse>>
{
    public async Task<ErrorOr<GetCategoryArticlesResponse>> Handle(GetCategoryArticlesQuery request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (category == null) return Errors.Category.NotFound;

        var articlesList = await dbContext.ArticleCategories.AsNoTracking()
            .Where(e => e.CategoryId == request.Id)
            .Select(e => e.Article)
            .Where(e => e.IsHidden == false && e.RedirectArticleId == null)
            .Select(e => new GetCategoryArticlesElement(e.Id, e.Title))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new GetCategoryArticlesResponse(articlesList);
    }
}