using Application.Features.Categories.GetCategories;
using Application.Features.Navigations.GetNavigationTree;
using MediatR;
using WebApi.Extensions;

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
    }
}