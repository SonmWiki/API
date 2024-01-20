using ErrorOr;
using MediatR;

namespace Application.Features.Navigations.GetNavigationTree;

public record GetNavigationsTreeQuery : IRequest<ErrorOr<GetNavigationsTreeResponse>>;