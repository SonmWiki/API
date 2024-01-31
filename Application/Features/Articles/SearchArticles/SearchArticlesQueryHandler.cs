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
        var filteredArticles = dbContext.Articles
            .Where(e => e.RedirectArticleId == null && e.CurrentRevisionId != null)
            .Where(e => EF.Functions.ILike(e.Title, $"%{query.SearchTerm}%"));
                
        var totalCount = await filteredArticles.AsNoTracking().CountAsync(token);
        
        var pagedArticles = await filteredArticles
            .OrderByDescending(e => EF.Functions.TrigramsSimilarity(e.Title, query.SearchTerm))
            .Skip((query.Page-1) * query.PageSize)
            .Take(query.PageSize)
            .Select(e=> new SearchArticlesResponse.Element(e.Id, e.Title))
            .AsNoTracking()
            .ToListAsync(token);
        
        return new SearchArticlesResponse(query.Page, pagedArticles.Count, totalCount, pagedArticles);
    }
}