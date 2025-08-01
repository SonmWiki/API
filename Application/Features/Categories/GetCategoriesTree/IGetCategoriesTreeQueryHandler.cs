using ErrorOr;

namespace Application.Features.Categories.GetCategoriesTree;

public interface IGetCategoriesTreeQueryHandler
{
    Task<ErrorOr<GetCategoriesTreeResponse>> Handle(GetCategoriesTreeQuery request,
        CancellationToken token);
}