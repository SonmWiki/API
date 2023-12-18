namespace Application.Features.Categories.GetCategoryArticles;

public record GetCategoryArticlesResponse(List<GetCategoryArticlesElement> Data);

public record GetCategoryArticlesElement(string Id, string Title);