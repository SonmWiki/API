using Application.Features.Navigations.UpdateNavigationsTree;

namespace WebApi.Features.Navigations.Requests;

public record UpdateNavigationsTreeRequest(List<UpdateNavigationsTreeCommand.Element> Data);