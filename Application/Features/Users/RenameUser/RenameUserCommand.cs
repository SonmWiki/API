using Application.Common.Messaging;

namespace Application.Features.Users.RenameUser;

public record RenameUserCommand(Guid Id, string Name) : ICommand<RenameUserResponse>;