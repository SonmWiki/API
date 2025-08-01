using ErrorOr;

namespace Application.Features.Articles.DeleteArticle;

public interface IDeleteArticleCommandHandler
{
    Task<ErrorOr<DeleteArticleResponse>> Handle(DeleteArticleCommand request, CancellationToken token);
}