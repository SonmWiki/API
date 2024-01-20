namespace Application.Features.Navigations.GetNavigationTree;

public record GetNavigationsTreeResponse(List<GetNavigationsTreeResponse.Element> Data)
{
    public record Element(int Id, int Weight, string Name, string? Uri, List<Element> Children);
}