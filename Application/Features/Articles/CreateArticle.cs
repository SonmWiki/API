using Application.Authorization.Abstractions;
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

namespace Application.Features.Articles;

public static class CreateArticle
{
    public record Request(string Title, string Content, List<string> CategoryIds);

    public record Command(string Title, string Content, List<string> CategoryIds) : IRequest<ErrorOr<Response>>;

    public record Response(string Id);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Title)
                .MaximumLength(128)
                .NotEmpty();
            RuleFor(v => v.Content)
                .NotEmpty();
        }
    }

    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/articles",
                async Task<IResult> (IMediator mediator, Request request) =>
                {
                    var command = new Command(request.Title, request.Content, request.CategoryIds);
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
            .RequireAuthorization(new AuthorizeAttribute {Roles = $"{Roles.Admin}, {Roles.Editor}, {Roles.User}"})
            .WithOpenApi();
    }

    public class CommandHandler(IApplicationDbContext dbContext, ISlugHelper slugHelper, IIdentityService identityService) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var id = slugHelper.GenerateSlug(command.Title);

            if (string.IsNullOrEmpty(id)) return Errors.Article.EmptyId;
            
            var existingArticle = await dbContext.Articles.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (existingArticle != null) return Errors.Article.DuplicateId;

            var article = new Article
            {
                Id = id,
                Title = command.Title,
                IsVisible = false
            };

            var revision = new Revision
            {
                ArticleId = id,
                Article = article,
                AuthorId = identityService.UserId!,
                Author = new Author{Id = identityService.UserId!, Name = identityService.UserName!},
                Content = command.Content,
                Timestamp = DateTime.Now,
                Status = RevisionStatus.Draft
            };

            await dbContext.Articles.AddAsync(article, cancellationToken);
            await dbContext.Revisions.AddAsync(revision, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(article.Id);
        }
    }
}