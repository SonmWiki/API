using Application.Authorization.Abstractions;
using Application.Common.Messaging;
using Application.Features.Users.GetUser;
using Application.Features.Users.RenameUser;
using Domain.Rbac;
using WebApi.Auth;
using WebApi.Extensions;
using WebApi.Features.Users.Requests;

namespace WebApi.Features.Users;

public static class UsersModule
{
    public static void AddUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/me",
                async Task<IResult> (IQueryHandler<GetUserQuery, GetUserResponse> getUserQueryHandler, IUserContext userContext, CancellationToken cancellationToken) =>
                {
                    var userId = userContext.UserId;
                    var response = await getUserQueryHandler.HandleAsync(new GetUserQuery(userId!.Value), cancellationToken);
                    return response.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("GetUser")
            .WithTags("Users")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces<GetUserResponse>()
            .WithOpenApi();

        app.MapPut("/api/users/{id}",
                async Task<IResult> (
                    Guid id,
                    ICommandHandler<RenameUserCommand, RenameUserResponse> commandHandler,
                    RenameUserRequest request,
                    CancellationToken cancellationToken) =>
                {
                    var command = new RenameUserCommand(id, request.Name);
                    var result = await commandHandler.HandleAsync(command, cancellationToken);
                    return result.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("RenameUser")
            .WithTags("Users")
            .Produces<RenameUserResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(Permissions.UserRename)
            .WithOpenApi();
    }
}