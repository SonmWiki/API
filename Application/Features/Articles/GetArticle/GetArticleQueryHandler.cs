using Application.Authorization.Abstractions;
using Application.Common.Constants;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.GetArticle;

public class GetArticleQueryHandler(
    IApplicationDbContext dbContext,
    IIdentityService identityService
) : IRequestHandler<GetArticleQuery, ErrorOr<GetArticleResponse>>
{
    public async Task<ErrorOr<GetArticleResponse>> Handle(GetArticleQuery query, CancellationToken token)
    {
        var article = await dbContext.Articles
            .Include(e => e.CurrentRevision)
            .ThenInclude(e => e.LatestReview)
            .Include(e => e.RedirectArticle)
            .ThenInclude(e => e.CurrentRevision)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == query.Id, token);

        if (article == null) return Errors.Article.NotFound;

        article = article.RedirectArticle ?? article;

        var revision = article.CurrentRevision;

        if (query.RevisionId != null)
        {
            var isInRole = await identityService.IsInRoleAsync(AuthorizationConstants.Roles.Editor) ||
                           await identityService.IsInRoleAsync(AuthorizationConstants.Roles.Admin);
            revision = await dbContext.Revisions
                .Include(e => e.LatestReview)
                .FirstOrDefaultAsync(e => e.Id == query.RevisionId && (isInRole || e.LatestReview != null), token);
            if (revision == null) return Errors.Revision.NotFound;
        }

        var categories = new List<GetArticleResponse.Category>();

        if (revision != null)
        {
            await dbContext.Revisions.Entry(revision).Collection(e => e.Categories).LoadAsync(token);
            categories = revision.Categories.Select(e => new GetArticleResponse.Category(e.Id, e.Name)).ToList();
        }

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
            .ToListAsync(token);

        return new GetArticleResponse(
            article.Id,
            article.Title,
            revision?.Content,
            contributors,
            revision?.Id,
            revision?.LatestReview?.Status,
            revision?.Timestamp,
            revision?.LatestReview?.ReviewTimestamp,
            categories
        );
    }
}