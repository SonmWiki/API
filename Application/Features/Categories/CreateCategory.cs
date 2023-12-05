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

public static class CreateCategory
{
    public record Request(string Name, string? ParentName);

    public record Command(string Name, string? ParentName) : IRequest<ErrorOr<Response>>;

    public record Response(string Name);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(128)
                .NotEmpty();
            RuleFor(v => v.ParentName)
                .MaximumLength(128);
        }
    }
    
    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/categories", async (IMediator mediator, Request request) =>
            {
                var command = new Command(request.Name, request.ParentName);
                var result = await mediator.Send(command);
                return result.MatchFirst(
                    value => Results.Created($"/api/categories/{request.Name}", value),
                    error => error.ToIResult()
                );
            })
            .WithName(nameof(CreateCategory))
            .WithTags("Category")
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithOpenApi();
    }
    
    public class CommandHandler(IApplicationDbContext dbContext) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var existingCategory = await dbContext.Categories.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == command.Name, cancellationToken);
            
            if (existingCategory != null) return Errors.Category.DuplicateName;

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

            var entity = new Category
            {
                Id = command.Name,
                Parent = parent
            };

            dbContext.Categories.Add(entity);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(entity.Id);
        }
    }
}