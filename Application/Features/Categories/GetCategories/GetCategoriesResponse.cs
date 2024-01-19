namespace Application.Features.Categories.GetCategories;

public record GetCategoriesResponse(List<GetCategoriesResponse.Element> Data)
{
    public record Element(string Id, string Name, string? ParentId);
}
