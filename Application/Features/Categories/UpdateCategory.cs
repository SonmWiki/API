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
using Slugify;

namespace Application.Features.Categories;

public static class UpdateCategory
{
    public record Request(string Name, string? ParentId);

    public record Command(string Id, string Name, string? ParentId) : IRequest<ErrorOr<Response>>;

    public record Response(string Id);

    public class UpdateCategoryCommandValidator : AbstractValidator<Command>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(128)
                .NotEmpty();
        }
    }

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/categories/{id}",
                async (string id, IMediator mediator, Request request) =>
                {
                    var command = new Command(id, request.Name, request.ParentId);
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

    public class CommandHandler(IApplicationDbContext dbContext, ISlugHelper slugHelper) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Categories.FindAsync(new object[] {command.Id}, cancellationToken);

            if (entity == null) return Errors.Category.NotFound;

            var updatedId = slugHelper.GenerateSlug(command.Name);

            if (string.IsNullOrEmpty(updatedId)) return Errors.Category.EmptyId;

            if (entity.Id != updatedId && await dbContext.Categories.AnyAsync(e => e.Id == updatedId, cancellationToken))
                return Errors.Category.DuplicateId;
            
            Category? parent;
            if (string.IsNullOrEmpty(command.ParentId))
            {
                parent = null;
            }
            else
            {
                var existingParent = await dbContext.Categories
                    .FirstOrDefaultAsync(e => e.Id == command.ParentId, cancellationToken);

                if (existingParent == null) return Errors.Category.ParentNotFound;

                parent = existingParent;
            }

            entity.Id = updatedId;
            entity.Name = command.Name;
            entity.Parent = parent;

            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(entity.Id);
        }
    }
}