using ErrorOr;

namespace Application.Features.Categories.GetCategoryArticles;

public interface IGetCategoryArticlesQueryHandler
{
    Task<ErrorOr<GetCategoryArticlesResponse>> Handle(GetCategoryArticlesQuery request,
        CancellationToken token);
}