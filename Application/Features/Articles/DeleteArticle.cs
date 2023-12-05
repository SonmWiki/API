using Application.Data;
using Application.Extensions;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Features.Articles;

public static class DeleteArticle
{
    public record Command(string Title) : IRequest<ErrorOr<Response>>;

    public record Response(string Title);
    
    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/articles/{title}",
            async (string title, IMediator mediator) =>
            {
                var command = new Command(title);
                var result = await mediator.Send(new Command(title));
                return result.MatchFirst(
                    value => Results.Ok(value),
                    error => error.ToIResult()
                );
            })            
            .WithName(nameof(DeleteArticle))
            .WithTags("Article")
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi();
    }
    
    public class CommandHandler(IApplicationDbContext dbContext) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var article = await dbContext.Articles.FindAsync(new object[] { request.Title }, cancellationToken);
            if (article == null) return Errors.Article.NotFound;
            dbContext.Articles.Remove(article);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new Response(article.Id);
        }
    }
}