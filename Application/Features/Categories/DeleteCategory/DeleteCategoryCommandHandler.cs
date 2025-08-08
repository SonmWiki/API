using Application.Common.Caching;
using Application.Common.Constants;
using Application.Common.Messaging;
using Application.Data;
using Application.Features.Articles.DeleteArticle;
using ErrorOr;

namespace Application.Features.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler(
    IApplicationDbContext dbContext,
    ICacheService cacheService
) : ICommandHandler<DeleteCategoryCommand, DeleteCategoryResponse>
{
    public async Task<ErrorOr<DeleteCategoryResponse>> HandleAsync(DeleteCategoryCommand deleteCategoryCommand,
        CancellationToken token)
    {
        var category = await dbContext.Categories.FindAsync(new object[] {deleteCategoryCommand.Id}, token);
        if (category == null) return Errors.Category.NotFound;
        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(token);

        await cacheService.RemoveAsync(CachingKeys.Categories.CategoriesAll, token);
        await cacheService.RemoveAsync(CachingKeys.Categories.CategoriesTree, token);
        await cacheService.RemoveAsync(CachingKeys.Categories.CategoryArticlesById(category.Id), token);

        return new DeleteCategoryResponse(category.Id);
    }
}