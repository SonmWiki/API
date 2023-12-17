using Application.Common.Constants;
using Application.Data;
using Application.Extensions;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using static Application.Common.Constants.AuthorizationConstants;

namespace Application.Features.Articles;

public static class DeleteArticle
{
    public record Command(string Id) : IRequest<ErrorOr<Response>>;

    public record Response(string Id);

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/articles/{id}",
                async (string id, IMediator mediator) =>
                {
                    var command = new Command(id);
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName(nameof(DeleteArticle))
            .WithTags("Article")
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}"})
            .WithOpenApi();
    }

    public class CommandHandler(IApplicationDbContext dbContext) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var article = await dbContext.Articles.FindAsync(new object[] {request.Id}, cancellationToken);
            if (article == null) return Errors.Article.NotFound;
            dbContext.Articles.Remove(article);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new Response(article.Id);
        }
    }
}