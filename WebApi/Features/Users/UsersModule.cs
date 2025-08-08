using Application.Authorization.Abstractions;
using Application.Features.Users.GetUser;
using WebApi.Extensions;

namespace WebApi.Features.Users;

public static class UsersModule
{
    public static void AddUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/me",
                async Task<IResult> (IGetUserCommandHandler getUserCommandHandler, IUserContext userContext, CancellationToken cancellationToken) =>
                {
                    var userId = userContext.UserId;
                    var response = await getUserCommandHandler.Handle(new GetUserCommand(userId!.Value), cancellationToken);
                    return response.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("GetUser")
            .WithTags("Users")
            .RequireAuthorization()
            .Produces<GetUserResponse>()
            .WithOpenApi();
    }
}