using Application.Data;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.GetPendingRevisionsCount;

public class GetPendingRevisionsCountQueryHandler
    (IApplicationDbContext dbContext) : IRequestHandler<GetPendingRevisionsCountQuery, ErrorOr<GetPendingRevisionsCountResponse>>
{
    public async Task<ErrorOr<GetPendingRevisionsCountResponse>> Handle(GetPendingRevisionsCountQuery query,
        CancellationToken token)
    {
        var pendingRevisionsCount = await dbContext.Revisions
            .CountAsync(e => e.LatestReviewId == null, cancellationToken: token);
        
        return new GetPendingRevisionsCountResponse(pendingRevisionsCount);
    }
}