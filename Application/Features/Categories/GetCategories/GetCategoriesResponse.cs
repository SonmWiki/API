namespace Application.Features.Categories.GetCategories;

public record GetCategoriesResponse(List<GetCategoriesResponseElement> Data);

public record GetCategoriesResponseElement(string Id, string Name, string? ParentId);