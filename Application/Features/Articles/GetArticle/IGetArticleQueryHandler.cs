using ErrorOr;

namespace Application.Features.Articles.GetArticle;

public interface IGetArticleQueryHandler
{
    Task<ErrorOr<GetArticleResponse>> Handle(GetArticleQuery query, CancellationToken token);
}