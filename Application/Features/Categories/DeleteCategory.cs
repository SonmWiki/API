using Application.Data;
using Application.Extensions;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Features.Categories;

public static class DeleteCategory
{
    public record Command(string Name) : IRequest<ErrorOr<Response>>;

    public record Response(string Name);

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/categories/{name}",
                async (string name, IMediator mediator) =>
                {
                    var command = new Command(name);
                    var result = await mediator.Send(new Command(name));
                    return result.MatchFirst(
                        value => Results.Ok(value),
                        error => error.ToIResult()
                    );
                })
            .WithName(nameof(DeleteCategory))
            .WithTags("Category")
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi();
    }

    public class CommandHandler(IApplicationDbContext dbContext) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var category = await dbContext.Categories.FindAsync(new object[] {command.Name}, cancellationToken);
            if (category == null) return Errors.Category.NotFound;
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new Response(category.Id);
        }
    }
}