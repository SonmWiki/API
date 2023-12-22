using ErrorOr;
using MediatR;

namespace Application.Features.Authors.EditAuthor;

public record EditAuthorCommand(string Id, string Name) : IRequest<ErrorOr<EditAuthorResponse>>;