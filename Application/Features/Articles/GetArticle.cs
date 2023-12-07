using Application.Data;
using Application.Extensions;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles;

public static class GetArticle
{
    public record Query(string Id) : IRequest<ErrorOr<Response>>;

    public record Response(
        string Id,
        string Title,
        string Content,
        string RevisionAuthor,
        DateTime Timestamp,
        List<string> Categories
    );

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/articles/{id}",
                async Task<IResult> (string id, IMediator mediator) =>
                {
                    var result = await mediator.Send(new Query(id));
                    return result.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName(nameof(GetArticle))
            .WithTags("Article")
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi();
    }

    public class QueryHandler(IApplicationDbContext dbContext) : IRequestHandler<Query, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var article = await dbContext.Articles.AsNoTracking()
                .Include(e=>e.RedirectArticle)
                .FirstOrDefaultAsync(e => e.IsVisible == true && e.Id == request.Id, cancellationToken);

            if (article == null) return Errors.Article.NotFound;

            article = article.RedirectArticle ?? article;

            var articleCategoriesIds = await dbContext.ArticleCategories.AsNoTracking()
                .Where(e => e.ArticleId == article.Id)
                .Select(e => e.CategoryId)
                .ToListAsync(cancellationToken);

            var revision = await dbContext.Revisions.AsNoTracking()
                .Include(e => e.Article)
                .Where(e => e.Article.Id == article.Id && e.Status == RevisionStatus.Active)
                .FirstOrDefaultAsync(cancellationToken);

            if (revision == null) return Errors.Article.NotFound;

            return new Response
            (
                article.Id,
                article.Title,
                revision.Content,
                revision.Author,
                (DateTime) revision.ReviewTimestamp!,
                articleCategoriesIds
            );
        }
    }
}