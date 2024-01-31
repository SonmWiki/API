namespace Application.Features.Articles.EditArticle;

public class ArticleEditedEvent
{
    public required string Id { get; init; }
    public required string Content { get; init; }
    public required List<string> CategoryIds { get; init; }
}