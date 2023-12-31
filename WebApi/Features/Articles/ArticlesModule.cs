﻿using Application.Extensions;
using Application.Features.Articles.CreateArticle;
using Application.Features.Articles.DeleteArticle;
using Application.Features.Articles.EditArticle;
using Application.Features.Articles.GetArticle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using WebApi.Extensions;
using WebApi.Features.Articles.Requests;
using static Application.Common.Constants.AuthorizationConstants;

namespace WebApi.Features.Articles;

public static class ArticlesModule
{
    public static void AddArticlesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/articles",
                async Task<IResult> (IMediator mediator, CreateArticleRequest request) =>
                {
                    var command =
                        new CreateArticleCommand(request.Title, request.Content,
                            request.CategoryIds);
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Created($"/api/articles/{value.Id}", value),
                        error => error.ToIResult()
                    );
                })
            .WithName("CreateArticle")
            .WithTags("Article")
            .Produces<CreateArticleResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}, {Roles.User}"})
            .WithOpenApi();
        
        app.MapGet("/api/articles/{id}",
                async Task<IResult> (string id, IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetArticleQuery(id));
                    return result.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("GetArticle")
            .WithTags("Article")
            .Produces<GetArticleResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi();
        
        app.MapPut("/api/articles/{id}",
                async Task<IResult> (string id, IMediator mediator, EditArticleRequest request) =>
                {
                    var command =
                        new EditArticleCommand(id, request.Title, request.Content,
                            request.CategoryIds);
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Created($"/api/articles/{value.Id}", value),
                        error => error.ToIResult()
                    );
                })
            .WithName("EditArticle")
            .WithTags("Article")
            .Produces<EditArticleResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}, {Roles.User}"})
            .WithOpenApi();
        
        app.MapDelete("/api/articles/{id}",
                async (string id, IMediator mediator) =>
                {
                    var command = new DeleteArticleCommand(id);
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("DeleteArticle")
            .WithTags("Article")
            .Produces<DeleteArticleResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}"})
            .WithOpenApi();
    }
}