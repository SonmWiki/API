using Application.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authors.EditAuthor;

public class EditAuthorCommandHandler(
    IApplicationDbContext dbContext
) : IEditAuthorCommandHandler
{
    public async Task<ErrorOr<EditAuthorResponse>> Handle(EditAuthorCommand command, CancellationToken token)
    {
        var author = await dbContext.Authors.FirstOrDefaultAsync(e => e.Id == command.Id, token);

        if (author == null) return Errors.Author.NotFound;

        author.Name = command.Name;

        await dbContext.SaveChangesAsync(token);

        // var authorEditedEvent = new AuthorEditedEvent
        // {
        //     Id = author.Id,
        //     Name = author.Name
        // };
        // await publisher.Publish(authorEditedEvent, token);

        return new EditAuthorResponse(author.Id);
    }
}