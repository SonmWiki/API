using ErrorOr;

namespace Application.Features.Articles.CreateArticle;

public interface ICreateArticleCommandHandler
{
    Task<ErrorOr<CreateArticleResponse>> Handle(CreateArticleCommand command, CancellationToken token);
}