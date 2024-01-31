using ErrorOr;
using MediatR;

namespace Application.Features.Articles.SearchArticles;

public record SearchArticlesQuery : IRequest<ErrorOr<SearchArticlesResponse>>
{
    public string SearchTerm { get; }
    public int Page { get; }
    public int PageSize { get; }

    public SearchArticlesQuery(string SearchTerm, int Page, int PageSize)
    {
        this.SearchTerm = SearchTerm;
        this.Page = Page < 1 ? 1 : Page;
        this.PageSize = PageSize is < 0 or > 100 ? 100 : PageSize;
    }
}