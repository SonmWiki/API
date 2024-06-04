using Application.Authorization.Abstractions;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.SetRedirect;

public class SetRedirectCommandHandler(
    IApplicationDbContext dbContext,
    IPublisher publisher
) : IRequestHandler<SetRedirectCommand, ErrorOr<SetRedirectResponse>>
{
    public async Task<ErrorOr<SetRedirectResponse>> Handle(SetRedirectCommand command,
        CancellationToken token)
    {
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

        var revisionReviewedEvent = new RedirectSetEvent
        {
            ArticleId = article.Id,
            RedirectId = redirectArticle.Id,
        };
        await publisher.Publish(revisionReviewedEvent, token);

        return new SetRedirectResponse(redirectArticle.Id);
    }
}