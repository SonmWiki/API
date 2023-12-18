namespace Application.Features.Articles.GetArticle;

public record GetArticleResponse(
    string Id,
    string Title,
    string Content,
    List<GetArticleResponse.Author> Contributors,
    DateTime Timestamp,
    List<string> Categories
)
{
    public record Author(string Id, string Name);
}