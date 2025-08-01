using ErrorOr;

namespace Application.Features.Categories.CreateCategory;

public interface ICreateCategoryCommandHandler
{
    Task<ErrorOr<CreateCategoryResponse>> Handle(CreateCategoryCommand command, CancellationToken token);
}