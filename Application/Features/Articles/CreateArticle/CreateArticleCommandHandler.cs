﻿using Application.Authorization.Abstractions;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace Application.Features.Articles.CreateArticle;

public class CreateArticleCommandHandler(
        IApplicationDbContext dbContext,
        ISlugHelper slugHelper,
        IIdentityService identityService
    )
    : IRequestHandler<CreateArticleCommand, ErrorOr<CreateArticleResponse>>
{
    public async Task<ErrorOr<CreateArticleResponse>> Handle(CreateArticleCommand command, CancellationToken cancellationToken)
    {
        var id = slugHelper.GenerateSlug(command.Title);

        if (string.IsNullOrEmpty(id)) return Errors.Article.EmptyId;
            
        var existingArticle = await dbContext.Articles.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (existingArticle != null) return Errors.Article.DuplicateId;

        var article = new Article
        {
            Id = id,
            Title = command.Title,
            IsHidden = true
        };
        
        var categories = await dbContext.Categories
            .Where(e => command.CategoryIds.Contains(e.Id))
            .ToListAsync(cancellationToken: cancellationToken);
        
        var revision = new Revision
        {
            ArticleId = id,
            Article = default!,
            AuthorId = identityService.UserId!,
            Author = default!,
            Title = command.Title,
            Content = command.Content,
            Categories = categories,
            Timestamp = DateTime.Now.ToUniversalTime(),
            Status = RevisionStatus.Submitted
        };

        await dbContext.Articles.AddAsync(article, cancellationToken);
        await dbContext.Revisions.AddAsync(revision, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateArticleResponse(article.Id);
    }
}