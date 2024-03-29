﻿using Application.Features.Navigations.GetNavigationTree;
using Application.Features.Navigations.UpdateNavigationsTree;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using WebApi.Extensions;
using WebApi.Features.Navigations.Requests;
using static Application.Common.Constants.AuthorizationConstants;

namespace WebApi.Features.Navigations;

public static class NavigationsModule
{
    public static void AddNavigationsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/navigations/tree",
                async Task<IResult> (IMediator mediator) =>
                {
                    var response = await mediator.Send(new GetNavigationsTreeQuery());
                    return response.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("GetNavigationsTree")
            .WithTags("Navigations")
            .Produces<GetNavigationsTreeResponse>()
            .WithOpenApi();

        app.MapPut("/api/navigations/tree",
                async Task<IResult> (IMediator mediator, UpdateNavigationsTreeRequest request) =>
                {
                    var command = new UpdateNavigationsTreeCommand(request.Data);
                    var response = await mediator.Send(command);
                    return response.MatchFirst(
                        value => Results.Ok(),
                        error => error.ToIResult()
                    );
                })
            .WithName("UpdateNavigationsTree")
            .WithTags("Navigations")
            .Produces<GetNavigationsTreeResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}"})
            .WithOpenApi();
    }
}