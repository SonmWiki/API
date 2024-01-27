using Application.Features.Navigations.GetNavigationTree;
using ErrorOr;
using MediatR;

namespace Application.Features.Navigations.UpdateNavigationsTree;

public record UpdateNavigationsTreeCommand(List<UpdateNavigationsTreeCommand.Element> Data) : IRequest<ErrorOr<UpdateNavigationsTreeResponse>>
{
    public record Element(string Name, string? Uri, string? Icon, List<Element> Children);
}