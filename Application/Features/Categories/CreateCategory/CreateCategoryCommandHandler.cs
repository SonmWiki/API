using Application.Common.Caching;
using Application.Common.Constants;
using Application.Common.Utils;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace Application.Features.Categories.CreateCategory;

public class CreateCategoryCommandHandler(
    IApplicationDbContext dbContext,
    ISlugHelper slugHelper,
    ICacheService cacheService,
    IValidator<CreateCategoryCommand> validator
) : ICreateCategoryCommandHandler
{
    public async Task<ErrorOr<CreateCategoryResponse>> Handle(CreateCategoryCommand command, CancellationToken token)
    {
        var validationResult = ValidatorHelper.Validate(validator, command);
        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var id = slugHelper.GenerateSlug(command.Name);

        if (string.IsNullOrEmpty(id)) return Errors.Category.EmptyId;

        var existingCategory = await dbContext.Categories
            .FirstOrDefaultAsync(e => e.Id == id, token);

        if (existingCategory != null) return Errors.Category.DuplicateId;

        Category? parent;
        if (string.IsNullOrEmpty(command.ParentId))
        {
            parent = null;
        }
        else
        {
            var existingParent = await dbContext.Categories
                .FirstOrDefaultAsync(e => e.Id == command.ParentId, token);

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
        await dbContext.SaveChangesAsync(token);

        await cacheService.RemoveAsync(CachingKeys.Categories.CategoriesAll, token);
        await cacheService.RemoveAsync(CachingKeys.Categories.CategoriesTree, token);

        return new CreateCategoryResponse(entity.Id);
    }
}