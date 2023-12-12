using Application.Data;
using Application.Extensions;
using Domain.Entities;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Slugify;
using static Application.Common.Constants.AuthorizationConstants;

namespace Application.Features.Categories;

public static class CreateCategory
{
    public record Request(string Name, string? ParentId);

    public record Command(string Name, string? ParentId) : IRequest<ErrorOr<Response>>;

    public record Response(string Id);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(128)
                .NotEmpty();
        }
    }

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/categories", async (IMediator mediator, Request request) =>
            {
                var command = new Command(request.Name, request.ParentId);
                var result = await mediator.Send(command);
                return result.MatchFirst(
                    value => Results.Created($"/api/categories/{value.Id}", value),
                    error => error.ToIResult()
                );
            })
            .WithName(nameof(CreateCategory))
            .WithTags("Category")
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}"})
            .WithOpenApi();
    }

    public class CommandHandler(IApplicationDbContext dbContext, ISlugHelper slugHelper) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var id = slugHelper.GenerateSlug(command.Name);

            if (string.IsNullOrEmpty(id)) return Errors.Category.EmptyId;

            var existingCategory = await dbContext.Categories
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (existingCategory != null) return Errors.Category.DuplicateId;

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

            var entity = new Category
            {
                Id = id,
                Name = command.Name,
                Parent = parent
            };

            dbContext.Categories.Add(entity);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(entity.Id);
        }
    }
}