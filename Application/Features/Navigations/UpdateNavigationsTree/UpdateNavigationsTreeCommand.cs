using Application.Common.Messaging;

namespace Application.Features.Navigations.UpdateNavigationsTree;

public record UpdateNavigationsTreeCommand(List<UpdateNavigationsTreeCommand.Element> Data) : ICommand<UpdateNavigationsTreeResponse>
{
    public record Element(string Name, string? Uri, string? Icon, List<Element> Children);
}