using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.GetArticle;

public class GetArticleQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetArticleQuery, ErrorOr<GetArticleResponse>>
{
    public async Task<ErrorOr<GetArticleResponse>> Handle(GetArticleQuery query, CancellationToken cancellationToken)
    {
        var article = await dbContext.Articles.AsNoTracking()
            .Include(e=>e.RedirectArticle)
            .FirstOrDefaultAsync(e => e.IsVisible == true && e.Id == query.Id, cancellationToken);

        if (article == null) return Errors.Article.NotFound;

        article = article.RedirectArticle ?? article;

        var articleCategoriesIds = await dbContext.ArticleCategories.AsNoTracking()
            .Where(e => e.ArticleId == article.Id)
            .Select(e => e.CategoryId)
            .ToListAsync(cancellationToken);

        var revision = await dbContext.Revisions.AsNoTracking()
            .Where(e => e.ArticleId == article.Id && e.Status == RevisionStatus.Active)
            .FirstOrDefaultAsync(cancellationToken);

        if (revision == null) return Errors.Article.NotFound;
            
        var contributors = await dbContext.Revisions.AsNoTracking()
            .Include(e => e.Author)
            .Where(e => e.ArticleId == article.Id && (e.Status == RevisionStatus.Accepted || e.Status == RevisionStatus.Active))
            .OrderBy(e=>e.ReviewTimestamp)
            .Select(e => new GetArticleResponse.Author(e.Author.Id, e.Author.Name))
            .Distinct()
            .ToListAsync(cancellationToken);

        return new GetArticleResponse
        (
            article.Id,
            article.Title,
            revision.Content,
            contributors,
            (DateTime) revision.ReviewTimestamp!,
            articleCategoriesIds
        );
    }
}