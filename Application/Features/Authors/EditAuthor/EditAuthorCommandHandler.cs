using Application.Common.Messaging;
using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authors.EditAuthor;

public class EditAuthorCommandHandler(
    IApplicationDbContext dbContext
) : ICommandHandler<EditAuthorCommand, EditAuthorResponse>
{
    public async Task<ErrorOr<EditAuthorResponse>> HandleAsync(EditAuthorCommand command, CancellationToken token)
    {
        var author = await dbContext.Authors.FirstOrDefaultAsync(e => e.Id == command.Id, token);

        if (author == null) return Errors.Author.NotFound;

        author.Name = command.Name;

        await dbContext.SaveChangesAsync(token);

        return new EditAuthorResponse(author.Id);
    }
}