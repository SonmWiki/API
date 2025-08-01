using ErrorOr;

namespace Application.Features.Articles.GetPendingRevisionsCount;

public interface IGetPendingRevisionsCountQueryHandler
{
    Task<ErrorOr<GetPendingRevisionsCountResponse>> Handle(GetPendingRevisionsCountQuery query,
        CancellationToken token);
}