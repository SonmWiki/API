using Application.Common.Messaging;

namespace Application.Features.Authors.EditAuthor;

public record EditAuthorCommand(string Id, string Name) : ICommand<EditAuthorResponse>;