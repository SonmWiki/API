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

namespace Application.Features.Articles;

public static class CreateArticle
{
    public record Request(string Title);

    public record Command(string Title) : IRequest<ErrorOr<Response>>;

    public record Response(string Title);

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
                        value => Results.Created($"/api/articles/{command.Title}", value),
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

    public class CommandHandler(IApplicationDbContext dbContext) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var existingArticle = await dbContext.Articles.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == command.Title, cancellationToken);
            if (existingArticle != null) return Errors.Article.DuplicateTitle;

            var article = new Article()
            {
                Id = command.Title,
                IsVisible = false
            };

            await dbContext.Articles.AddAsync(article, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(article.Id);
        }
    }
}