using Application.Data;
using Application.Extensions;
using Domain.Entities;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories;

public static class UpdateCategory
{
    public record Request(string Name, string? ParentName);

    public record Command(string OriginalName, string UpdatedName, string? ParentName) : IRequest<ErrorOr<Response>>;

    public record Response(string Name);

    public class UpdateCategoryCommandValidator : AbstractValidator<Command>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(v => v.UpdatedName)
                .MaximumLength(128)
                .NotEmpty();
        }
    }
    
    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/categories/{name}",
            async (string name, IMediator mediator, Request request) =>
            {
                var command = new Command(name, request.Name, request.ParentName);
                var response = await mediator.Send(command);
                return response.MatchFirst(
                    value => Results.Ok(value),
                    error => error.ToIResult()
                );
            })
            .WithName(nameof(UpdateCategory))
            .WithTags("Category")
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi();
    }
    
    public class CommandHandler(IApplicationDbContext dbContext) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Categories.FindAsync(new object[] {command.OriginalName}, cancellationToken);

            if (entity == null) return Errors.Category.NotFound;

            Category? parent;
            if (string.IsNullOrEmpty(command.ParentName))
            {
                parent = null;
            }
            else
            {
                var existingParent = await dbContext.Categories.AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == command.ParentName, cancellationToken);
                
                if (existingParent == null) return Errors.Category.ParentNotFound;
                
                parent = existingParent;
            }

            entity.Id = command.UpdatedName;
            entity.Parent = parent;

            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(entity.Id);
        }
    }
}