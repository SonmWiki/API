﻿using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace Application.Features.Categories.CreateCategory;

public class CreateCategoryCommandHandler(IApplicationDbContext dbContext, ISlugHelper slugHelper) : IRequestHandler<CreateCategoryCommand, ErrorOr<CreateCategoryResponse>>
{
    public async Task<ErrorOr<CreateCategoryResponse>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var id = slugHelper.GenerateSlug(command.Name);

        if (string.IsNullOrEmpty(id)) return Errors.Category.EmptyId;

        var existingCategory = await dbContext.Categories
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (existingCategory != null) return Errors.Category.DuplicateId;

        Category? parent;
        if (string.IsNullOrEmpty(command.ParentId))
        {
            parent = null;
        }
        else
        {
            var existingParent = await dbContext.Categories
                .FirstOrDefaultAsync(e => e.Id == command.ParentId, cancellationToken);

            if (existingParent == null) return Errors.Category.ParentNotFound;

            parent = existingParent;
        }

        var entity = new Category
        {
            Id = id,
            Name = command.Name,
            Parent = parent
        };

        dbContext.Categories.Add(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateCategoryResponse(entity.Id);
    }
}