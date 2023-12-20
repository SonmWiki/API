using ErrorOr;
using MediatR;

namespace Application.Features.Authors.UpdateAuthor;

public record UpdateAuthorCommand(string Id, string Name) : IRequest<ErrorOr<UpdateAuthorResponse>>;