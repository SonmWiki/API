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
        if (!string.IsNullOrWhiteSpace(query.Id))
            return await GetByArticleId(query.Id, token);
        
        return await GetByRevisionId(query.RevisionId.GetValueOrDefault(), token);
    }

    private async Task<ErrorOr<GetArticleResponse>> GetByArticleId(string id, CancellationToken token)
    {
        var article = await dbContext.Articles
            .Include(e => e.CurrentRevision)
            .Include(e => e.RedirectArticle)
            .ThenInclude(e => e.CurrentRevision)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, token);

        if (article == null) return Errors.Article.NotFound;

        article = article.RedirectArticle ?? article;

        var categories = await GetCategories(article.CurrentRevision, token);
        var contributors = await GetContributors(article, token);

        return new GetArticleResponse(
            article.Id,
            article.Title,
            article.CurrentRevision?.Content,
            contributors,
            article.CurrentRevision?.Id,
            article.CurrentRevision?.LatestReview?.Status,
            article.CurrentRevision?.Timestamp,
            article.CurrentRevision?.LatestReview?.ReviewTimestamp,
            categories
        );
    }

    private async Task<ErrorOr<GetArticleResponse>> GetByRevisionId(Guid id, CancellationToken token)
    {
        var isInRole = await identityService.IsInRoleAsync(AuthorizationConstants.Roles.Editor) ||
                       await identityService.IsInRoleAsync(AuthorizationConstants.Roles.Admin);
        var revision = await dbContext.Revisions
            .Include(e => e.LatestReview)
            .Include(e => e.Article)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id && (isInRole || e.LatestReview != null), token);

        if (revision == null) return Errors.Revision.NotFound;

        var categories = await GetCategories(revision, token);
        var contributors = await GetContributors(revision.Article, token);

        return new GetArticleResponse(
            revision.Article.Id,
            revision.Article.Title,
            revision.Content,
            contributors,
            revision.Id,
            revision.LatestReview?.Status,
            revision.Timestamp,
            revision.LatestReview?.ReviewTimestamp,
            categories
        );
    }

    private async Task<List<GetArticleResponse.Category>> GetCategories(Revision? revision, CancellationToken token)
    {
        if (revision == null) return [];
        await dbContext.Revisions.Entry(revision).Collection(e => e.Categories).LoadAsync(token);
        return revision.Categories.Select(e => new GetArticleResponse.Category(e.Id, e.Name)).ToList();
    }

    private async Task<List<GetArticleResponse.Author>> GetContributors(Article article, CancellationToken token)
    {
        return await dbContext.Revisions
            .Include(e => e.Author)
            .Include(e => e.LatestReview)
            .Where(e => e.ArticleId == article.Id
                        && e.LatestReview != null
                        && e.LatestReview.Status == ReviewStatus.Accepted
                        && e.Timestamp <= (article.CurrentRevision == null
                            ? DateTime.UtcNow
                            : article.CurrentRevision.Timestamp))
            .OrderBy(e => e.Timestamp)
            .Select(e => new GetArticleResponse.Author(e.Author.Id, e.Author.Name))
            .Distinct()
            .AsNoTracking()
            .ToListAsync(token);
    }
}