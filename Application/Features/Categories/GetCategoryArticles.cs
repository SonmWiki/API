using Application.Data;
using Application.Extensions;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories;

public static class GetCategoryArticles
{
    public record Query(string Id) : IRequest<ErrorOr<Response>>;

    public record Response(List<Response.Element> Data)
    {
        public record Element(string Id, string Title);
    }

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categories/{id}/articles",
                async Task<IResult> (string id, IMediator mediator) =>
                {
                    var query = new Query(id);
                    var response = await mediator.Send(query);
                    return response.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName(nameof(GetCategoryArticles))
            .WithTags("Category", "Article")
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi();
    }

    public class QueryHandler(IApplicationDbContext dbContext) : IRequestHandler<Query, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var category = await dbContext.Categories.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (category == null) return Errors.Category.NotFound;

            var articlesList = await dbContext.ArticleCategories.AsNoTracking()
                .Where(e => e.CategoryId == request.Id)
                .Select(e => e.Article)
                .Where(e => e.IsVisible == true && e.RedirectArticleId == null)
                .Select(e => new Response.Element(e.Id, e.Title))
                .ToListAsync(cancellationToken);

            return new Response(articlesList);
        }
    }
}