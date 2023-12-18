using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace Application.Features.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler(IApplicationDbContext dbContext, ISlugHelper slugHelper) : IRequestHandler<UpdateCategoryCommand, ErrorOr<UpdateCategoryResponse>>
{
    public async Task<ErrorOr<UpdateCategoryResponse>> Handle(UpdateCategoryCommand updateCategoryCommand, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Categories.FindAsync(new object[] {updateCategoryCommand.Id}, cancellationToken);

        if (entity == null) return Errors.Category.NotFound;

        var updatedId = slugHelper.GenerateSlug(updateCategoryCommand.Name);

        if (string.IsNullOrEmpty(updatedId)) return Errors.Category.EmptyId;

        if (entity.Id != updatedId && await dbContext.Categories.AnyAsync(e => e.Id == updatedId, cancellationToken))
            return Errors.Category.DuplicateId;

        Category? parent;
        if (string.IsNullOrEmpty(updateCategoryCommand.ParentId))
        {
            parent = null;
        }
        else
        {
            var existingParent = await dbContext.Categories
                .FirstOrDefaultAsync(e => e.Id == updateCategoryCommand.ParentId, cancellationToken);

            if (existingParent == null) return Errors.Category.ParentNotFound;

            parent = existingParent;
        }

        entity.Id = updatedId;
        entity.Name = updateCategoryCommand.Name;
        entity.Parent = parent;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateCategoryResponse(entity.Id);
    }
}