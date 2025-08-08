namespace Application.Features.Users.GetUser;
using ErrorOr;

public interface IGetUserCommandHandler
{
    Task<ErrorOr<GetUserResponse>> Handle(GetUserCommand command, CancellationToken token);
}