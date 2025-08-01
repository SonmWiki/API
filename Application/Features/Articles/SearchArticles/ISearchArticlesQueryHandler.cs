using ErrorOr;

namespace Application.Features.Articles.SearchArticles;

public interface ISearchArticlesQueryHandler
{
    Task<ErrorOr<SearchArticlesResponse>> Handle(SearchArticlesQuery query,  CancellationToken token);
}