using ErrorOr;

namespace Application.Features.Navigations.UpdateNavigationsTree;

public interface IUpdateNavigationsTreeCommandHandler
{
    Task<ErrorOr<UpdateNavigationsTreeResponse>> Handle(UpdateNavigationsTreeCommand request,
        CancellationToken token);
}