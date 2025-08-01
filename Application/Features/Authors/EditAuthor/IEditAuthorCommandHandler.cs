using ErrorOr;

namespace Application.Features.Authors.EditAuthor;

public interface IEditAuthorCommandHandler
{
    Task<ErrorOr<EditAuthorResponse>> Handle(EditAuthorCommand command, CancellationToken token);
}