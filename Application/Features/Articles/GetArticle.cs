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
    public record Query(string Title) : IRequest<ErrorOr<Response>>;

    public record Response(
        string Title,
        string Content,
        string RevisionAuthor,
        DateTime Timestamp,
        List<string> Categories
    );

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/articles/{title}",
                async Task<IResult> (string title, IMediator mediator) =>
                {
                    var result = await mediator.Send(new Query(title));
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
                .FirstOrDefaultAsync(e => e.IsVisible == true && e.Id == request.Title, cancellationToken);

            if (article == null) return Errors.Article.NotFound;

            var articleId = article.RedirectArticleId ?? article.Id;

            var articleCategoriesIds = await dbContext.ArticleCategories.AsNoTracking()
                .Where(e => e.ArticleId == articleId)
                .Select(e => e.CategoryId)
                .ToListAsync(cancellationToken);

            var revision = await dbContext.Revisions.AsNoTracking()
                .Include(e => e.Article)
                .Where(e => e.Article.Id == articleId && e.Status == RevisionStatus.Active)
                .FirstOrDefaultAsync(cancellationToken);

            if (revision == null) return Errors.Article.NotFound;

            return new Response
            (
                articleId,
                revision.Content,
                revision.Author,
                (DateTime) revision.ReviewTimestamp!,
                articleCategoriesIds
            );
        }
    }
}