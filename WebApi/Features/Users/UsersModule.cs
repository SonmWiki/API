using Application.Authorization.Abstractions;
using Application.Common.Messaging;
using Application.Features.Users.GetUser;
using WebApi.Extensions;

namespace WebApi.Features.Users;

public static class UsersModule
{
    public static void AddUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/me",
                async Task<IResult> (IQueryHandler<GetUserQuery, GetUserResponse> getUserCommandHandler, IUserContext userContext, CancellationToken cancellationToken) =>
                {
                    var userId = userContext.UserId;
                    var response = await getUserCommandHandler.HandleAsync(new GetUserQuery(userId!.Value), cancellationToken);
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