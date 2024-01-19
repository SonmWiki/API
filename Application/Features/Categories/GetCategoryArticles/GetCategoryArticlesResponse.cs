namespace Application.Features.Categories.GetCategoryArticles;

public record GetCategoryArticlesResponse(List<GetCategoryArticlesResponse.Element> Data)
{
    public record Element(string Id, string Title);
}
