using ErrorOr;
using MediatR;

namespace Application.Features.Authors.CreateAuthor;

public record CreateAuthorCommand(string Id, string Name) : IRequest<ErrorOr<CreateAuthorResponse>>;