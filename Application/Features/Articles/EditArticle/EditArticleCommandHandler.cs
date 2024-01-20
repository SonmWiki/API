using Application.Authorization.Abstractions;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.EditArticle;

public class EditArticleCommandHandler
    (IApplicationDbContext dbContext, ICurrentUserService identityService) : IRequestHandler<EditArticleCommand,
        ErrorOr<EditArticleResponse>>
{
    public async Task<ErrorOr<EditArticleResponse>> Handle(EditArticleCommand request, CancellationToken token)
    {
        var article = await dbContext.Articles.Include(e=>e.RedirectArticle).FirstOrDefaultAsync(e => e.Id == request.Id, token);
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
            AuthorId = identityService.UserId!,
            Author = default!,
            Content = request.Content,
            Categories = requestCategories,
            Timestamp = DateTime.UtcNow
        };

        await dbContext.Revisions.AddAsync(revision, token);
        await dbContext.SaveChangesAsync(token);

        return new EditArticleResponse(article.Id);
    }
}