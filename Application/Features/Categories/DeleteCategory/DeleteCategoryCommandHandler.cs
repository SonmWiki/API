using Application.Data;
using ErrorOr;
using MediatR;

namespace Application.Features.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler
    (IApplicationDbContext dbContext) : IRequestHandler<DeleteCategoryCommand, ErrorOr<DeleteCategoryResponse>>
{
    public async Task<ErrorOr<DeleteCategoryResponse>> Handle(DeleteCategoryCommand deleteCategoryCommand,
        CancellationToken token)
    {
        var category = await dbContext.Categories.FindAsync(new object[] {deleteCategoryCommand.Id}, token);
        if (category == null) return Errors.Category.NotFound;
        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(token);
        return new DeleteCategoryResponse(category.Id);
    }
}