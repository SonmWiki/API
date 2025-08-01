using ErrorOr;

namespace Application.Features.Articles.GetPendingRevisions;

public interface IGetPendingRevisionsQueryHandler
{
    Task<ErrorOr<GetPendingRevisionsResponse>> Handle(GetPendingRevisionsQuery query, CancellationToken token);
}