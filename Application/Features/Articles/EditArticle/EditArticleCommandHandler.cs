using Application.Authorization.Abstractions;
using Application.Common.Messaging;
using Application.Common.Utils;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.EditArticle;

public class EditArticleCommandHandler(
    IApplicationDbContext dbContext,
    IUserContext userContext,
    IValidator<EditArticleCommand> validator
) : ICommandHandler<EditArticleCommand, EditArticleResponse>
{
    public async Task<ErrorOr<EditArticleResponse>> HandleAsync(EditArticleCommand request, CancellationToken token)
    {
        var validationResult = ValidatorHelper.Validate(validator, request);
        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var article = await dbContext.Articles.Include(e => e.RedirectArticle)
            .FirstOrDefaultAsync(e => e.Id == request.Id, token);
        if (article == null) return Errors.Article.NotFound;

        if (article.RedirectArticle != null) article = article.RedirectArticle;

        var requestCategories = await dbContext.Categories
            .Where(e => request.CategoryIds.Contains(e.Id))
            .ToListAsync(token);

        var revision = new Revision
        {
            Id = default!,
            ArticleId = article.Id,
            Article = default!,
            AuthorId = userContext.UserId!.Value,
            Author = default!,
            AuthorsNote = request.AuthorsNote,
            Content = request.Content,
            Categories = requestCategories,
            Timestamp = DateTime.UtcNow
        };

        await dbContext.Revisions.AddAsync(revision, token);
        await dbContext.SaveChangesAsync(token);

        return new EditArticleResponse(article.Id);
    }
}