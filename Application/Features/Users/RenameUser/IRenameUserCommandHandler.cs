using ErrorOr;

namespace Application.Features.Users.RenameUser;

public interface IRenameUserCommandHandler
{
    Task<ErrorOr<RenameUserResponse>> Handle(RenameUserCommand command, CancellationToken token);
}