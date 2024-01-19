using Application.Authorization.Abstractions;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Articles.EditArticle;

public class EditArticleCommandHandler(
        IApplicationDbContext dbContext,
        IIdentityService identityService
    )
    : IRequestHandler<EditArticleCommand, ErrorOr<EditArticleResponse>>
{
    public async Task<ErrorOr<EditArticleResponse>> Handle(EditArticleCommand request,
        CancellationToken cancellationToken)
    {
        var article = await dbContext.Articles.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (article == null) return Errors.Article.NotFound;

        var requestCategories = await dbContext.Categories
            .Where(e => request.CategoryIds.Contains(e.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        var revision = new Revision
        {
            ArticleId = request.Id,
            Article = default!,
            AuthorId = identityService.UserId!,
            Author = default!,
            Content = request.Content,
            Categories = requestCategories,
            Timestamp = DateTime.Now.ToUniversalTime(),
        };

        await dbContext.Revisions.AddAsync(revision, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new EditArticleResponse(request.Id);
    }
}