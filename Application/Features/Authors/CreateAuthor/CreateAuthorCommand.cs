using Application.Common.Messaging;

namespace Application.Features.Authors.CreateAuthor;

public record CreateAuthorCommand(string Id, string Name) : ICommand<CreateAuthorResponse>;