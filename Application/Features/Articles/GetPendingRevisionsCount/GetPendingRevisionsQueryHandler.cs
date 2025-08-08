using Application.Common.Messaging;
using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.GetPendingRevisionsCount;

public class GetPendingRevisionsCountQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetPendingRevisionsCountQuery, GetPendingRevisionsCountResponse>
{
    public async Task<ErrorOr<GetPendingRevisionsCountResponse>> HandleAsync(GetPendingRevisionsCountQuery query,
        CancellationToken token)
    {
        var pendingRevisionsCount = await dbContext.Revisions
            .CountAsync(e => e.LatestReviewId == null, cancellationToken: token);
        
        return new GetPendingRevisionsCountResponse(pendingRevisionsCount);
    }
}