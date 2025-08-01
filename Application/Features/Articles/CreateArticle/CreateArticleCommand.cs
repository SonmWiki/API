using ErrorOr;

namespace Application.Features.Articles.CreateArticle;

public record CreateArticleCommand(
    string Title,
    string Content,
    string AuthorsNote,
    List<string> CategoryIds
);