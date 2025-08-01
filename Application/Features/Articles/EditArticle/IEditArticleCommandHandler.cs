using ErrorOr;

namespace Application.Features.Articles.EditArticle;

public interface IEditArticleCommandHandler
{
    Task<ErrorOr<EditArticleResponse>> Handle(EditArticleCommand request, CancellationToken token);
}