using Application.Data;
using Application.Extensions;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories;

public static class GetCategories
{
    public record Query : IRequest<ErrorOr<Response>>;

    public record Response(List<Response.Element> Data)
    {
        public record Element(string Name, string? ParentName);
    }

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categories",
                async Task<IResult> (IMediator mediator) =>
                {
                    var response = await mediator.Send(new Query());
                    return response.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName(nameof(GetCategories))
            .WithTags("Category")
            .Produces<Response>()
            .WithOpenApi();
    }

    public class QueryHandler(IApplicationDbContext dbContext) : IRequestHandler<Query, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var list = await dbContext.Categories
                .Select(e => new Response.Element(e.Id, e.ParentId))
                .ToListAsync(cancellationToken: cancellationToken);
            
            return new Response(list);
        }
    }
}