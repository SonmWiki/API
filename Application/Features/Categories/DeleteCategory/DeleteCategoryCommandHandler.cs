using Application.Data;
using ErrorOr;
using MediatR;

namespace Application.Features.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteCategoryCommand, ErrorOr<DeleteCategoryResponse>>
{
    public async Task<ErrorOr<DeleteCategoryResponse>> Handle(DeleteCategoryCommand deleteCategoryCommand, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories.FindAsync(new object[] {deleteCategoryCommand.Id}, cancellationToken);
        if (category == null) return Errors.Category.NotFound;
        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteCategoryResponse(category.Id);
    }
}
