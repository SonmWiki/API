using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace Application.Features.Categories.EditCategory;

public class EditCategoryCommandHandler(IApplicationDbContext dbContext, ISlugHelper slugHelper) : IRequestHandler<EditCategoryCommand, ErrorOr<EditCategoryResponse>>
{
    public async Task<ErrorOr<EditCategoryResponse>> Handle(EditCategoryCommand editCategoryCommand, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Categories.FindAsync(new object[] {editCategoryCommand.Id}, cancellationToken);

        if (entity == null) return Errors.Category.NotFound;

        var updatedId = slugHelper.GenerateSlug(editCategoryCommand.Name);

        if (string.IsNullOrEmpty(updatedId)) return Errors.Category.EmptyId;

        if (entity.Id != updatedId && await dbContext.Categories.AnyAsync(e => e.Id == updatedId, cancellationToken))
            return Errors.Category.DuplicateId;

        Category? parent;
        if (string.IsNullOrEmpty(editCategoryCommand.ParentId))
        {
            parent = null;
        }
        else
        {
            var existingParent = await dbContext.Categories
                .FirstOrDefaultAsync(e => e.Id == editCategoryCommand.ParentId, cancellationToken);

            if (existingParent == null) return Errors.Category.ParentNotFound;

            parent = existingParent;
        }

        entity.Id = updatedId;
        entity.Name = editCategoryCommand.Name;
        entity.Parent = parent;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new EditCategoryResponse(entity.Id);
    }
}