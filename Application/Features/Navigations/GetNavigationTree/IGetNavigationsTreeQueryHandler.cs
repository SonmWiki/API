using ErrorOr;

namespace Application.Features.Navigations.GetNavigationTree;

public interface IGetNavigationsTreeQueryHandler
{
    Task<ErrorOr<GetNavigationsTreeResponse>> Handle(GetNavigationsTreeQuery request,
        CancellationToken token);
}