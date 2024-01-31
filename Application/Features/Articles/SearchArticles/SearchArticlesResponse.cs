namespace Application.Features.Articles.SearchArticles;

public record SearchArticlesResponse(
    int Page,
    int Count,
    int TotalPages,
    List<SearchArticlesResponse.Element> Data)
{
    public record Element(string Id, string ArticleTitle);
}