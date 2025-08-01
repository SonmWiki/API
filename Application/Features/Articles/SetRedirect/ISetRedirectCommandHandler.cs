using ErrorOr;

namespace Application.Features.Articles.SetRedirect;

public interface ISetRedirectCommandHandler
{
    Task<ErrorOr<SetRedirectResponse>> Handle(SetRedirectCommand command,
        CancellationToken token);
}