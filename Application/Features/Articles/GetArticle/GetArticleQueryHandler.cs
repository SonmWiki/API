using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.GetArticle;

public class GetArticleQueryHandler
    (IApplicationDbContext dbContext) : IRequestHandler<GetArticleQuery, ErrorOr<GetArticleResponse>>
{
    public async Task<ErrorOr<GetArticleResponse>> Handle(GetArticleQuery query, CancellationToken cancellationToken)
    {
        var article = await dbContext.Articles
            .Include(e => e.CurrentRevision)
            .ThenInclude(e => e.LatestReview)
            .Include(e => e.RedirectArticle)
            .ThenInclude(e => e.CurrentRevision)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == query.Id, cancellationToken);

        if (article == null) return Errors.Article.NotFound;

        article = article.RedirectArticle ?? article;

        var revision = article.CurrentRevision;

        if (query.RevisionId != null)
        {
            revision = await dbContext.Revisions
                .Include(e => e.LatestReview)
                .FirstOrDefaultAsync(e => e.Id == query.RevisionId && e.LatestReview != null,
                    cancellationToken: cancellationToken);
            if (revision == null) return Errors.Revision.NotFound;
        }

        var articleCategoriesIds = await dbContext.ArticleCategories
            .Where(e => e.ArticleId == article.Id)
            .Select(e => e.CategoryId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var contributors = await dbContext.Revisions
            .Include(e => e.Author)
            .Include(e => e.LatestReview)
            .Where(e => e.ArticleId == article.Id 
                        && e.LatestReview != null 
                        && e.LatestReview.Status == ReviewStatus.Accepted 
                        && e.Timestamp <= (revision == null ? DateTime.UtcNow : revision.Timestamp))
            .OrderBy(e => e.Timestamp)
            .Select(e => new GetArticleResponse.Author(e.Author.Id, e.Author.Name))
            .Distinct()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new GetArticleResponse(
            article.Id,
            article.Title,
            revision?.Content,
            contributors,
            revision?.Id,
            revision?.LatestReview?.Status,
            revision?.Timestamp,
            revision?.LatestReview?.ReviewTimestamp,
            articleCategoriesIds
        );
    }
}