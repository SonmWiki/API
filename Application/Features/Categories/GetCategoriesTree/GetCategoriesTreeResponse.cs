namespace Application.Features.Categories.GetCategoriesTree;

public record GetCategoriesTreeResponse(List<GetCategoriesTreeResponse.Element> Data)
{
    public record Element(string Id, string Name, List<Element> Children);
}