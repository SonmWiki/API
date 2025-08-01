using ErrorOr;

namespace Application.Features.Articles.GetRevisionHistory;

public interface IGetRevisionHistoryQueryHandler
{
    Task<ErrorOr<GetRevisionHistoryResponse>> Handle(GetRevisionHistoryQuery query,
        CancellationToken token);
}