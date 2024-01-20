using Application.Authorization.Abstractions;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.ReviewRevision;

public class ReviewRevisionCommandHandler(
        IApplicationDbContext dbContext,
        ICurrentUserService identityService
    )
    : IRequestHandler<ReviewRevisionCommand, ErrorOr<ReviewRevisionResponse>>
{
    public async Task<ErrorOr<ReviewRevisionResponse>> Handle(ReviewRevisionCommand command,
        CancellationToken token)
    {
        var revision = await dbContext.Revisions
            .Include(e => e.Article)
            .Include(e => e.Categories)
            .Include(e => e.LatestReview)
            .FirstOrDefaultAsync(e => e.Id == command.RevisionId, token);

        if (revision == null) return Errors.Revision.NotFound;

        var article = revision.Article;

        var review = new Review
        {
            Id = default!,
            ReviewerId = identityService.UserId!,
            Reviewer = default!,
            Status = command.Status,
            Message = command.Review,
            ReviewTimestamp = DateTime.UtcNow,
            RevisionId = revision.Id,
            Revision = revision
        };

        revision.LatestReview = review;
        
        if (command.Status is ReviewStatus.Removed)
            revision.Content = "[REDACTED]";

        if (article.CurrentRevision == revision && command is {Status: ReviewStatus.Removed or ReviewStatus.Rejected})
        {
            var rollbackRevision = await dbContext.Revisions
                .Include(e => e.LatestReview)
                .Where(e => e.Id != revision.Id && e.ArticleId == article.Id && e.LatestReview != null &&
                            e.LatestReview.Status == ReviewStatus.Accepted)
                .OrderByDescending(e => e.Timestamp)
                .FirstOrDefaultAsync(token);

            article.CurrentRevision = rollbackRevision == null ? null : revision;
        }

        if (command is {Status: ReviewStatus.Accepted})
            article.CurrentRevision = revision;

        await dbContext.SaveChangesAsync(token);
        return new ReviewRevisionResponse(review.Id);
    }
}