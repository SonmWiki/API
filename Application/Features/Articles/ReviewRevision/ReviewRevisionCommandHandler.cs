using Application.Authorization.Abstractions;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.ReviewRevision;

public class ReviewRevisionCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService identityService,
    IPublisher publisher
) : IRequestHandler<ReviewRevisionCommand, ErrorOr<ReviewRevisionResponse>>
{
    public async Task<ErrorOr<ReviewRevisionResponse>> Handle(ReviewRevisionCommand command,
        CancellationToken token)
    {
        var revision = await dbContext.Revisions
            .Include(e => e.Article)
            .ThenInclude(e => e.CurrentRevision)
            .ThenInclude(e => e.Categories)
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

        ArticleChangedRevisionEvent? articleChangedRevisionEvent = null;

        revision.LatestReview = review;

        if (command.Status is ReviewStatus.Removed)
        {
            revision.Content = "[REDACTED]";
            revision.AuthorsNote = "[REDACTED]";
        }
        
        if (article.CurrentRevision == revision && command is {Status: ReviewStatus.Removed or ReviewStatus.Rejected})
        {
            var rollbackRevision = await dbContext.Revisions
                .Include(e => e.LatestReview)
                .Include(e => e.Categories)
                .Where(e => e.Id != revision.Id && e.ArticleId == article.Id && e.LatestReview != null &&
                            e.LatestReview.Status == ReviewStatus.Accepted)
                .OrderByDescending(e => e.Timestamp)
                .FirstOrDefaultAsync(token);

            ChangeArticleRevision(article, rollbackRevision, out articleChangedRevisionEvent);
        }

        if (command is {Status: ReviewStatus.Accepted})
        {
            ChangeArticleRevision(article, revision, out articleChangedRevisionEvent);
        }

        await dbContext.SaveChangesAsync(token);
        
        var revisionReviewedEvent = new RevisionReviewedEvent
        {
            ArticleId = article.Id,
            RevisionId = revision.Id,
            Status = command.Status,
            Review = command.Review,
        };
        await publisher.Publish(revisionReviewedEvent, token);
        
        if (articleChangedRevisionEvent != null) await publisher.Publish(articleChangedRevisionEvent, token);

        return new ReviewRevisionResponse(review.Id);
    }
    
    private static void ChangeArticleRevision(Article article, Revision? revision, out ArticleChangedRevisionEvent articleChangedRevisionEvent)
    {
        var previousRevisionId = article.CurrentRevision?.Id;
        var previousRevisionCategoriesIds = article.CurrentRevision?.Categories.Select(e => e.Id).ToList() ?? new List<string>();
        article.CurrentRevision = revision;
        articleChangedRevisionEvent = new ArticleChangedRevisionEvent
        {
            ArticleId = article.Id,
            PreviousRevisionId = previousRevisionId,
            CurrentRevisionId = article.CurrentRevision?.Id,
            PreviousRevisionCategoryIds = previousRevisionCategoriesIds,
            CurrentRevisionCategoryIds = revision?.Categories.Select(e => e.Id).ToList() ?? new List<string>()
        };
    }
}