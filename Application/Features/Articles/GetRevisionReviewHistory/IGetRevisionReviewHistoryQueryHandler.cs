using ErrorOr;

namespace Application.Features.Articles.GetRevisionReviewHistory;

public interface IGetRevisionReviewHistoryQueryHandler
{
    Task<ErrorOr<GetRevisionReviewHistoryResponse>> Handle(GetRevisionReviewHistoryQuery query,
        CancellationToken token);
}