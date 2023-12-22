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
            .Include(e=>e.RedirectArticle)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == query.Id && e.IsHidden == false, cancellationToken);

        if (article == null) return Errors.Article.NotFound;

        article = article.RedirectArticle ?? article;

        var revision = await dbContext.Revisions
            .Where(e => e.ArticleId == article.Id && e.Status == RevisionStatus.Active)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (revision == null) return Errors.Article.NotFound;
        
        var articleCategoriesIds = await dbContext.ArticleCategories
            .Where(e => e.ArticleId == article.Id)
            .Select(e => e.CategoryId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
            
        var contributors = await dbContext.Revisions
            .Include(e => e.Author)
            .Where(e => e.ArticleId == article.Id && (e.Status == RevisionStatus.Accepted || e.Status == RevisionStatus.Active))
            .OrderBy(e=>e.ReviewTimestamp)
            .Select(e => new GetArticleResponse.Author(e.Author.Id, e.Author.Name))
            .Distinct()
            .AsNoTracking()
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