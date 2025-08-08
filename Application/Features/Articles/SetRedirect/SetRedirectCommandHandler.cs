using Application.Common.Caching;
using Application.Common.Constants;
using Application.Common.Messaging;
using Application.Common.Utils;
using Application.Data;
using ErrorOr;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.SetRedirect;

public class SetRedirectCommandHandler(
    IApplicationDbContext dbContext,
    ICacheService cacheService,
    IValidator<SetRedirectCommand> validator
) : ICommandHandler<SetRedirectCommand, SetRedirectResponse>
{
    public async Task<ErrorOr<SetRedirectResponse>> HandleAsync(SetRedirectCommand command, CancellationToken token)
    {
        var validationResult = ValidatorHelper.Validate(validator, command);
        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var article = await dbContext.Articles.FirstOrDefaultAsync(e => e.Id == command.ArticleId, token);
        if (article == null) return Errors.Article.NotFound;
        if (article.RedirectArticleId != null) return Errors.Article.RedirectExists;

        var redirectArticle = await dbContext.Articles.FirstOrDefaultAsync(e => e.Id == command.RedirectId, token);
        if (redirectArticle == null) return Errors.Article.NotFound;
        if (article.RedirectArticleId != null) return Errors.Article.UnfitRedirectTarget;

        await dbContext.Articles
            .Where(e => e.RedirectArticleId == article.Id)
            .ExecuteUpdateAsync(p =>
                    p.SetProperty(x => x.RedirectArticleId, redirectArticle.Id), token
            );

        await dbContext.Revisions
            .Where(e => e.ArticleId == article.Id)
            .ExecuteUpdateAsync(p => p.SetProperty(x => x.ArticleId, redirectArticle.Id), token);

        article.RedirectArticleId = redirectArticle.Id;

        await dbContext.SaveChangesAsync(token);

        await cacheService.RemoveAsync(CachingKeys.Articles.ArticleById(article.Id), token);
        await cacheService.RemoveAsync(CachingKeys.Articles.ArticleById(redirectArticle.Id), token);

        return new SetRedirectResponse(redirectArticle.Id);
    }
}