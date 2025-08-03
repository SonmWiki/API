using Application.Authorization.Abstractions;
using Application.Common.Utils;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace Application.Features.Articles.CreateArticle;

public class CreateArticleCommandHandler(
    IApplicationDbContext dbContext,
    ISlugHelper slugHelper,
    ICurrentUserService identityService,
    IValidator<CreateArticleCommand> validator
) : ICreateArticleCommandHandler
{
    public async Task<ErrorOr<CreateArticleResponse>> Handle(CreateArticleCommand command, CancellationToken token)
    {
        var validationResult = ValidatorHelper.Validate(validator, command);
        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var id = slugHelper.GenerateSlug(command.Title);

        if (string.IsNullOrEmpty(id)) return Errors.Article.EmptyId;

        var existingArticle = await dbContext.Articles.FirstOrDefaultAsync(e => e.Id == id, token);
        if (existingArticle != null) return Errors.Article.DuplicateId;

        var article = new Article
        {
            Id = id,
            Title = command.Title
        };

        var categories = await dbContext.Categories
            .Where(e => command.CategoryIds.Contains(e.Id))
            .ToListAsync(token);

        var revision = new Revision
        {
            Id = default!,
            ArticleId = id,
            Article = default!,
            AuthorId = identityService.UserId!,
            Author = default!,
            AuthorsNote = command.AuthorsNote,
            Content = command.Content,
            Categories = categories,
            Timestamp = DateTime.UtcNow
        };

        await dbContext.Articles.AddAsync(article, token);
        await dbContext.Revisions.AddAsync(revision, token);

        await dbContext.SaveChangesAsync(token);

        return new CreateArticleResponse(article.Id);
    }
}