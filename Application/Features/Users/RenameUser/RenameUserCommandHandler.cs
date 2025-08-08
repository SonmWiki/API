using Application.Common.Messaging;
using Application.Data;
using Application.Features.Users.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.RenameUser;

public class RenameUserCommandHandler(
    IApplicationDbContext dbContext
) : ICommandHandler<RenameUserCommand, RenameUserResponse>
{
    public async Task<ErrorOr<RenameUserResponse>> HandleAsync(RenameUserCommand command, CancellationToken token)
    {
        var author = await dbContext.Users.FirstOrDefaultAsync(e => e.Id == command.Id, token);

        if (author == null) return User.NotFound;

        author.Name = command.Name;

        await dbContext.SaveChangesAsync(token);

        return new RenameUserResponse(author.Id);
    }
}