using Application.Common.Messaging;
using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.GetRevisionReviewHistory;

public class GetRevisionReviewHistoryQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetRevisionReviewHistoryQuery, GetRevisionReviewHistoryResponse>
{
    public async Task<ErrorOr<GetRevisionReviewHistoryResponse>> HandleAsync(GetRevisionReviewHistoryQuery query,
        CancellationToken token)
    {
        if (!await dbContext.Revisions.AnyAsync(e => e.Id == query.Id, token)) return Errors.Revision.NotFound;

        var reviews = await dbContext.Reviews
            .Include(e => e.Reviewer)
            .Where(e => e.RevisionId == query.Id)
            .ToListAsync(token);

        return new GetRevisionReviewHistoryResponse(reviews.Select(e => new GetRevisionReviewHistoryResponse.Element(
                e.Id,
                new GetRevisionReviewHistoryResponse.Reviewer(e.Reviewer.Id, e.Reviewer.Name),
                e.Status,
                e.Message,
                e.ReviewTimestamp
            )).ToList()
        );
    }
}