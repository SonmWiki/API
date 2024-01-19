using Application.Data;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.GetRevisionHistory;

public class GetRevisionHistoryQueryHandler
    (IApplicationDbContext dbContext) : IRequestHandler<GetRevisionHistoryQuery, ErrorOr<GetRevisionHistoryResponse>>
{
    public async Task<ErrorOr<GetRevisionHistoryResponse>> Handle(GetRevisionHistoryQuery query,
        CancellationToken cancellationToken)
    {
        var article = await dbContext.Articles
            .Include(e => e.Revisions).ThenInclude(e => e.Author)
            .Include(e => e.Revisions).ThenInclude(e => e.LatestReview).ThenInclude(e => e.Reviewer)
            .Include(e => e.RedirectArticle)
            .ThenInclude(e => e.CurrentRevision)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == query.Id, cancellationToken);

        if (article == null) return Errors.Article.NotFound;

        article = article.RedirectArticle ?? article;


        return new GetRevisionHistoryResponse(Data:
            article.Revisions.Select(e => new GetRevisionHistoryResponse.Element(
                    e.Id,
                    new GetRevisionHistoryResponse.Author(e.Author.Id, e.Author.Name),
                    e.Timestamp,
                    e.LatestReview == null
                        ? null
                        : new GetRevisionHistoryResponse.Review(
                            Id: e.LatestReview.Id,
                            Reviewer: new GetRevisionHistoryResponse.Author(
                                e.LatestReview.Reviewer.Id,
                                e.LatestReview.Reviewer.Name
                            ),
                            Status: e.LatestReview.Status,
                            Message: e.LatestReview.Message,
                            ReviewTimestamp: e.LatestReview.ReviewTimestamp
                        )
                )
            ).ToList()
        );
    }
}