using ErrorOr;

namespace Application.Features.Categories.DeleteCategory;

public interface IDeleteCategoryCommandHandler
{
    Task<ErrorOr<DeleteCategoryResponse>> Handle(DeleteCategoryCommand deleteCategoryCommand,
        CancellationToken token);
}