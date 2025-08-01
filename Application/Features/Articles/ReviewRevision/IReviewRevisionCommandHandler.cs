using ErrorOr;

namespace Application.Features.Articles.ReviewRevision;

public interface IReviewRevisionCommandHandler
{
    Task<ErrorOr<ReviewRevisionResponse>> Handle(ReviewRevisionCommand command,
        CancellationToken token);
}