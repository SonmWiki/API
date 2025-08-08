using Application.Authorization.Abstractions;
using Application.Common.Caching;
using Application.Common.Constants;
using Application.Common.Messaging;
using Application.Common.Utils;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.ReviewRevision;

public class ReviewRevisionCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService identityService,
    ICacheService cacheService,
    IValidator<ReviewRevisionCommand> validator
) : ICommandHandler<ReviewRevisionCommand, ReviewRevisionResponse>
{
    public async Task<ErrorOr<ReviewRevisionResponse>> HandleAsync(ReviewRevisionCommand command,
        CancellationToken token)
    {
        var validationResult = ValidatorHelper.Validate(validator, command);
        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var revision = await dbContext.Revisions
            .Include(e => e.Article)
            .ThenInclude(e => e.CurrentRevision)
            .ThenInclude(e => e!.Categories)
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

        HashSet<string> affectedCategories = [];

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

            ChangeArticleRevision(article, rollbackRevision, out affectedCategories);
        }

        if (command is {Status: ReviewStatus.Accepted})
        {
            ChangeArticleRevision(article, revision, out affectedCategories);
        }

        await dbContext.SaveChangesAsync(token);

        await cacheService.RemoveAsync(CachingKeys.Articles.ArticleById(article.Id), token);
        foreach (var id in affectedCategories)
        {
            await cacheService.RemoveAsync(CachingKeys.Categories.CategoryArticlesById(id), token);
        }

        return new ReviewRevisionResponse(review.Id);
    }
    
    private static void ChangeArticleRevision(Article article, Revision? revision, out HashSet<string> affectedCategories)
    {
        var previousRevisionCategoriesIds = article.CurrentRevision?.Categories.Select(e => e.Id).ToHashSet() ?? [];

        article.CurrentRevision = revision;

        var currentRevisionCategoryIds = revision?.Categories.Select(e => e.Id).ToHashSet() ?? [];

        previousRevisionCategoriesIds.SymmetricExceptWith(currentRevisionCategoryIds);
        affectedCategories = previousRevisionCategoriesIds;
    }
}