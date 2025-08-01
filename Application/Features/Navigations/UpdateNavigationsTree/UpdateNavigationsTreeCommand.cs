namespace Application.Features.Navigations.UpdateNavigationsTree;

public record UpdateNavigationsTreeCommand(List<UpdateNavigationsTreeCommand.Element> Data)
{
    public record Element(string Name, string? Uri, string? Icon, List<Element> Children);
}