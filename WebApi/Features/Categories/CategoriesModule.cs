using Application.Features.Categories.CreateCategory;
using Application.Features.Categories.DeleteCategory;
using Application.Features.Categories.EditCategory;
using Application.Features.Categories.GetCategories;
using Application.Features.Categories.GetCategoriesTree;
using Application.Features.Categories.GetCategoryArticles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using WebApi.Extensions;
using WebApi.Features.Categories.Requests;
using static Application.Common.Constants.AuthorizationConstants;

namespace WebApi.Features.Categories;

public static class CategoriesModule
{
    public static void AddCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/categories", async (IMediator mediator, CreateCategoryRequest request) =>
            {
                var command = new CreateCategoryCommand(request.Name, request.ParentId);
                var result = await mediator.Send(command);
                return result.MatchFirst(
                    value => Results.Created($"/api/categories/{value.Id}", value),
                    error => error.ToIResult()
                );
            })
            .WithName("CreateCategory")
            .WithTags("Category")
            .Produces<CreateCategoryResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}"})
            .WithOpenApi();

        app.MapGet("/api/categories",
                async Task<IResult> (IMediator mediator) =>
                {
                    var response = await mediator.Send(new GetCategoriesQuery());
                    return response.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("GetCategories")
            .WithTags("Category")
            .Produces<GetCategoriesResponse>()
            .WithOpenApi();
        
        app.MapGet("/api/categories/tree",
                async Task<IResult> (IMediator mediator) =>
                {
                    var response = await mediator.Send(new GetCategoriesTreeQuery());
                    return response.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("GetCategoriesTree")
            .WithTags("Category")
            .Produces<GetCategoriesTreeResponse>()
            .WithOpenApi();

        app.MapGet("/api/categories/{id}/articles",
                async Task<IResult> (string id, IMediator mediator) =>
                {
                    var query = new GetCategoryArticlesQuery(id);
                    var response = await mediator.Send(query);
                    return response.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("GetCategoryArticles")
            .WithTags("Category", "Article")
            .Produces<GetCategoryArticlesResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi();

        app.MapPut("/api/categories/{id}",
                async (string id, IMediator mediator, UpdateCategoryRequest request) =>
                {
                    var command = new EditCategoryCommand(id, request.Name, request.ParentId);
                    var response = await mediator.Send(command);
                    return response.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("UpdateCategory")
            .WithTags("Category")
            .Produces<EditCategoryResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}"})
            .WithOpenApi();

        app.MapDelete("/api/categories/{id}",
                async (string id, IMediator mediator) =>
                {
                    var command = new DeleteCategoryCommand(id);
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName("DeleteCategory")
            .WithTags("Category")
            .Produces<DeleteCategoryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}"})
            .WithOpenApi();
    }
}