using Application.Data;
using ErrorOr;

namespace Application.Features.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler(
    IApplicationDbContext dbContext
) : IDeleteCategoryCommandHandler
{
    public async Task<ErrorOr<DeleteCategoryResponse>> Handle(DeleteCategoryCommand deleteCategoryCommand,
        CancellationToken token)
    {
        var category = await dbContext.Categories.FindAsync(new object[] {deleteCategoryCommand.Id}, token);
        if (category == null) return Errors.Category.NotFound;
        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(token);

        // var categoryDeletedEvent = new CategoryDeletedEvent {Id = category.Id};
        // await publisher.Publish(categoryDeletedEvent, token);

        return new DeleteCategoryResponse(category.Id);
    }
}