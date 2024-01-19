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
        var article = await dbContext.Articles
            .Include(e=>e.CurrentRevision)
            .Include(e=>e.RedirectArticle)
            .ThenInclude(e=>e != null ? e.CurrentRevision : null)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == query.Id, cancellationToken);
        
        if (article == null) return Errors.Article.NotFound;
        
        article = article.RedirectArticle ?? article;
        
        
        var articleCategoriesIds = await dbContext.ArticleCategories
            .Where(e => e.ArticleId == article.Id)
            .Select(e => e.CategoryId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
            
        var contributors = await dbContext.Revisions
            .Include(e => e.Author)
            .Include(e => e.LatestReview)
            .Where(e => e.ArticleId == article.Id && e.LatestReview != null && e.LatestReview.Status == ReviewStatus.Accepted )
            .OrderBy(e => e.LatestReview!.ReviewTimestamp)
            .Select(e => new GetArticleResponse.Author(e.Author.Id, e.Author.Name))
            .Distinct()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return new GetArticleResponse
        (
            article.Id,
            article.Title,
            article.CurrentRevision != null ? article.CurrentRevision.Content : "NoContent",
            contributors,
            article.CurrentRevision != null ? article.CurrentRevision.LatestReview!.ReviewTimestamp : DateTime.UtcNow,
            articleCategoriesIds
        );
    }
}