using Domain.Entities;

namespace Application.Features.Articles.ReviewRevision;

public record ReviewRevisionCommand(
    Guid RevisionId,
    ReviewStatus Status,
    string Review
);