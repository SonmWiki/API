using Domain.Entities;
using MediatR;

namespace Application.Features.Articles.ReviewRevision;

public class RevisionReviewedEvent : INotification
{
    public required string ArticleId { get; init; }
    public required Guid RevisionId { get; init; }
    public required ReviewStatus Status { get; init; }
    public required string Review { get; init; }
}