using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Features.Articles.ReviewRevision;

public record ReviewRevisionCommand(
        Guid RevisionId,
        ReviewStatus Status,
        string Review
    )
    : IRequest<ErrorOr<ReviewRevisionResponse>>;