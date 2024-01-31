using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace Application.Features.Categories.EditCategory;

public class EditCategoryCommandHandler(
    IApplicationDbContext dbContext,
    ISlugHelper slugHelper,
    IPublisher publisher
) : IRequestHandler<EditCategoryCommand, ErrorOr<EditCategoryResponse>>
{
    public async Task<ErrorOr<EditCategoryResponse>> Handle(EditCategoryCommand editCategoryCommand,
        CancellationToken token)
    {
        var entity = await dbContext.Categories.FindAsync(new object[] {editCategoryCommand.Id}, token);

        if (entity == null) return Errors.Category.NotFound;

        var updatedId = slugHelper.GenerateSlug(editCategoryCommand.Name);

        if (string.IsNullOrEmpty(updatedId)) return Errors.Category.EmptyId;

        if (entity.Id != updatedId && await dbContext.Categories.AnyAsync(e => e.Id == updatedId, token))
            return Errors.Category.DuplicateId;

        Category? parent;
        if (string.IsNullOrEmpty(editCategoryCommand.ParentId))
        {
            parent = null;
        }
        else
        {
            var existingParent = await dbContext.Categories
                .FirstOrDefaultAsync(e => e.Id == editCategoryCommand.ParentId, token);

            if (existingParent == null) return Errors.Category.ParentNotFound;

            parent = existingParent;
        }

        entity.Id = updatedId;
        entity.Name = editCategoryCommand.Name;
        entity.Parent = parent;

        await dbContext.SaveChangesAsync(token);

        var categoryEditedEvent = new CategoryEditedEvent
        {
            Id = entity.Id,
            Name = entity.Name,
            ParentId = entity.Parent?.Id
        };
        await publisher.Publish(categoryEditedEvent, token);

        return new EditCategoryResponse(entity.Id);
    }
}