using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.GetPendingRevisions;

public class GetPendingRevisionsQueryHandler(IApplicationDbContext dbContext) : IGetPendingRevisionsQueryHandler
{
    public async Task<ErrorOr<GetPendingRevisionsResponse>> Handle(GetPendingRevisionsQuery query, CancellationToken token)
    {
        var pendingRevisions = await dbContext.Revisions
            .Where(e => e.LatestReviewId == null)
            .OrderByDescending(e => e.Timestamp)
            .Select(e => new GetPendingRevisionsResponse.Element(
                    e.ArticleId,
                    e.Article.Title,
                    e.Id,
                    new GetPendingRevisionsResponse.Author(
                        e.AuthorId,
                        e.Author.Name
                    ),
                    e.AuthorsNote,
                    e.Timestamp
                )
            )
            .AsNoTracking()
            .ToListAsync(token);


        return new GetPendingRevisionsResponse(pendingRevisions);
    }
}