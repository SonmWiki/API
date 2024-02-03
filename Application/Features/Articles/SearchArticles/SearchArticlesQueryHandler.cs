using Application.Data;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.SearchArticles;

public class SearchArticlesQueryHandler
    (IApplicationDbContext dbContext) : IRequestHandler<SearchArticlesQuery, ErrorOr<SearchArticlesResponse>>
{
    public async Task<ErrorOr<SearchArticlesResponse>> Handle(SearchArticlesQuery query,  CancellationToken token)
    {
        var articles = dbContext.Articles
            .Where(e => e.RedirectArticleId == null && e.CurrentRevisionId != null);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            articles = articles.Where(e => EF.Functions.ILike(e.Title, $"%{query.SearchTerm}%"))
                    .OrderByDescending(e => EF.Functions.TrigramsSimilarity(e.Title, query.SearchTerm));
        }
        
        var totalCount = await articles.AsNoTracking().CountAsync(token);
        
        var pagedArticles = await articles
            .Skip((query.Page-1) * query.PageSize)
            .Take(query.PageSize)
            .Select(e=> new SearchArticlesResponse.Element(e.Id, e.Title))
            .AsNoTracking()
            .ToListAsync(token);
        
        return new SearchArticlesResponse(query.Page, pagedArticles.Count, totalCount, pagedArticles);
    }
}