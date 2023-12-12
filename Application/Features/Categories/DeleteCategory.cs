using Application.Data;
using Application.Extensions;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using static Application.Common.Constants.AuthorizationConstants;

namespace Application.Features.Categories;

public static class DeleteCategory
{
    public record Command(string Id) : IRequest<ErrorOr<Response>>;

    public record Response(string Id);

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/categories/{id}",
                async (string id, IMediator mediator) =>
                {
                    var command = new Command(id);
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName(nameof(DeleteCategory))
            .WithTags("Category")
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}"})
            .WithOpenApi();
    }

    public class CommandHandler(IApplicationDbContext dbContext) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var category = await dbContext.Categories.FindAsync(new object[] {command.Id}, cancellationToken);
            if (category == null) return Errors.Category.NotFound;
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new Response(category.Id);
        }
    }
}