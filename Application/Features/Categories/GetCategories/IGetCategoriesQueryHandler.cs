using ErrorOr;

namespace Application.Features.Categories.GetCategories;

public interface IGetCategoriesQueryHandler
{
    Task<ErrorOr<GetCategoriesResponse>> Handle(GetCategoriesQuery request,
        CancellationToken token);
}