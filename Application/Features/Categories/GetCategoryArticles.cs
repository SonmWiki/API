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
    public record Query(string Name) : IRequest<ErrorOr<List<Response>>>;

    public record Response(string Title);

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categories/{name}/articles",
                async Task<IResult> (string name, IMediator mediator) =>
                {
                    var response = await mediator.Send(new Query(name));
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

    public class QueryHandler(IApplicationDbContext dbContext) : IRequestHandler<Query, ErrorOr<List<Response>>>
    {
        public async Task<ErrorOr<List<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var category = await dbContext.Categories.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Name, cancellationToken);

            if (category == null) return Errors.Category.NotFound;
            
            var articlesList = await dbContext.ArticleCategories.AsNoTracking()
                .Where(e => e.CategoryId == request.Name)
                .Select(e => e.Article)
                .Where(e => e.IsVisible == true && e.RedirectArticleId == null)
                .Select(e => new Response(e.Id))
                .ToListAsync(cancellationToken);
            
            return articlesList;
        }
    }
}