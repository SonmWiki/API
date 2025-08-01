using ErrorOr;

namespace Application.Features.Authors.CreateAuthor;

public interface ICreateAuthorCommandHandler
{
    Task<ErrorOr<CreateAuthorResponse>> Handle(CreateAuthorCommand command, CancellationToken token);
}