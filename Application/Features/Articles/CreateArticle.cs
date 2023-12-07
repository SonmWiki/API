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

namespace Application.Features.Articles;

public static class CreateArticle
{
    public record Request(string Title);

    public record Command(string Title) : IRequest<ErrorOr<Response>>;

    public record Response(string Id);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Title)
                .MaximumLength(128)
                .NotEmpty();
        }
    }

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/articles",
                async Task<IResult> (IMediator mediator, Request request) =>
                {
                    var command = new Command(request.Title);
                    var result = await mediator.Send(command);
                    return result.MatchFirst(
                        value => Results.Created($"/api/articles/{value.Id}", value),
                        error => error.ToIResult()
                    );
                })
            .WithName(nameof(CreateArticle))
            .WithTags("Article")
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithOpenApi();
    }

    public class CommandHandler(IApplicationDbContext dbContext, ISlugHelper slugHelper) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var id = slugHelper.GenerateSlug(command.Title);

            if (string.IsNullOrEmpty(id)) return Errors.Article.EmptyId;
            
            var existingArticle = await dbContext.Articles
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (existingArticle != null) return Errors.Article.DuplicateId;

            var article = new Article()
            {
                Id = id,
                Title = command.Title,
                IsVisible = false
            };

            await dbContext.Articles.AddAsync(article, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(article.Id);
        }
    }
}