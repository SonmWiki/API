using Application.Features.Articles.CreateArticle;
using Application.Features.Articles.DeleteArticle;
using Application.Features.Articles.EditArticle;
using Application.Features.Articles.GetArticle;
using Application.Features.Articles.GetPendingRevisions;
using Application.Features.Articles.GetRevisionHistory;
using Application.Features.Articles.GetRevisionReviewHistory;
using Application.Features.Articles.ReviewRevision;
using Application.Features.Articles.SetRedirect;
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
                    var command = new CreateArticleCommand(request.Title, request.Content, request.CategoryIds);
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
                async Task<IResult> (string id, Guid? revisionId, IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetArticleQuery(id, revisionId));
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
                    var command = new EditArticleCommand(id, request.Content, request.CategoryIds);
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
        
        app.MapPut("/api/articles/{id}/redirect",
                async Task<IResult> (string id, IMediator mediator, SerArticleRedirectRequest request) =>
                {
                    var command = new SetRedirectCommand(id, request.RedirectArticleId);
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Created($"/api/articles/{value.Id}", true),
                        error => error.ToIResult()
                    );
                })
            .WithName("SetArticleRedirect")
            .WithTags("Article")
            .Produces<SetRedirectResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}"})
            .WithOpenApi();
        
        app.MapGet("/api/articles/revisions/pending",
                async Task<IResult> (IMediator mediator) =>
                {
                    var command = new GetPendingRevisionsQuery();
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("PendingRevisions")
            .WithTags("Article")
            .Produces<GetPendingRevisionsResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}"})
            .WithOpenApi();

        app.MapGet("/api/articles/{id}/revisions",
                async Task<IResult> (string id, IMediator mediator) =>
                {
                    var command = new GetRevisionHistoryQuery(id);
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("RevisionHistory")
            .WithTags("Article")
            .Produces<GetRevisionHistoryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi();

        app.MapGet("/api/articles/revisions/{id}/reviews",
                async Task<IResult> (Guid id, IMediator mediator) =>
                {
                    var command = new GetRevisionReviewHistoryQuery(id);
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("RevisionReviewHistory")
            .WithTags("Article")
            .Produces<GetRevisionReviewHistoryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi();

        app.MapPost("/api/articles/revisions/{id}/reviews",
                async Task<IResult> (Guid id, IMediator mediator, ReviewArticleRevisionRequest request) =>
                {
                    var command = new ReviewRevisionCommand(id, request.Status, request.Review);
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Created($"/api/articles/revisions/{id}/reviews/{value.Id}", value),
                        error => error.ToIResult()
                    );
                })
            .WithName("ReviewArticleRevision")
            .WithTags("Article")
            .Produces<ReviewRevisionResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}"})
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