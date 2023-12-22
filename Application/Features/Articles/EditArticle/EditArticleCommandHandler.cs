using Application.Authorization.Abstractions;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slugify;
using static Application.Common.Constants.AuthorizationConstants;

namespace Application.Features.Articles.EditArticle;

public class EditArticleCommandHandler(
    IApplicationDbContext dbContext,
    ISlugHelper slugHelper,
    IIdentityService identityService
)
    : IRequestHandler<EditArticleCommand, ErrorOr<EditArticleResponse>>
{
    public async Task<ErrorOr<EditArticleResponse>> Handle(EditArticleCommand request,
        CancellationToken cancellationToken)
    {
        var id = slugHelper.GenerateSlug(request.Title);

        if (string.IsNullOrEmpty(id)) return Errors.Article.EmptyId;

        var article = await dbContext.Articles
            .Include(e => e.ArticleCategories)
            .FirstOrDefaultAsync(e => e.Id == request.Id && e.IsHidden == false, cancellationToken);
        
        if (article == null) return Errors.Article.NotFound;

        if (id != request.Id)
        {
            var existingArticle = await dbContext.Articles.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (existingArticle != null) return Errors.Article.DuplicateId;

            article = new Article
            {
                Id = id,
                Title = request.Title,
                IsHidden = true
            };

            await dbContext.Articles.AddAsync(article, cancellationToken);
        }

        var principal = identityService.Principal;
        
        // TODO: Allow user to change title when generated slug unchanged
        if (principal != null && (principal.IsInRole(Roles.Admin) || principal.IsInRole(Roles.Editor))) 
            article.Title = request.Title;
        
        var activeRevision = await dbContext.Revisions.FirstOrDefaultAsync(e =>
                e.Status == RevisionStatus.Active && e.ArticleId == request.Id,
            cancellationToken: cancellationToken);

        if (activeRevision == null) return Errors.Article.NotFound;

        if (activeRevision.Content != request.Content)
        {
            var revision = new Revision
            {
                ArticleId = request.Id,
                Article = default!,
                AuthorId = identityService.UserId!,
                Author = default!,
                Content = request.Content,
                Timestamp = DateTime.Now.ToUniversalTime(),
                Status = RevisionStatus.Submitted
            };

            await dbContext.Revisions.AddAsync(revision, cancellationToken);
        }

        var categories = await dbContext.Categories
            .Where(e => request.CategoryIds.Contains(e.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        article.Categories.Clear();
        article.Categories = categories;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new EditArticleResponse(id);
    }
}