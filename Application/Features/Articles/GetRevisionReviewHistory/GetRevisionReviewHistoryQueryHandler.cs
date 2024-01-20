using Application.Data;
using Application.Features.Articles.Errors;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.GetRevisionReviewHistory;

public class GetRevisionReviewHistoryQueryHandler
    (IApplicationDbContext dbContext) : IRequestHandler<GetRevisionReviewHistoryQuery,
        ErrorOr<GetRevisionReviewHistoryResponse>>
{
    public async Task<ErrorOr<GetRevisionReviewHistoryResponse>> Handle(GetRevisionReviewHistoryQuery query,
        CancellationToken ct)
    {
        if (!await dbContext.Revisions.AnyAsync(e => e.Id == query.Id, ct)) return Errors.Revision.NotFound;
        
        var reviews = await dbContext.Reviews
            .Include(e => e.Reviewer)
            .Where(e => e.RevisionId == query.Id)
            .ToListAsync(ct);

        return new GetRevisionReviewHistoryResponse(Data:
            reviews.Select(e => new GetRevisionReviewHistoryResponse.Element(
                e.Id,
                new GetRevisionReviewHistoryResponse.Reviewer(e.Reviewer.Id, e.Reviewer.Name),
                e.Status,
                e.Message,
                e.ReviewTimestamp
            )).ToList()
        );
    }
}