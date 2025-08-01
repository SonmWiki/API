using Application.Data;
using Domain.Entities;
using ErrorOr;

namespace Application.Features.Authors.CreateAuthor;

public class CreateAuthorCommandHandler(
    IApplicationDbContext dbContext
) : ICreateAuthorCommandHandler
{
    public async Task<ErrorOr<CreateAuthorResponse>> Handle(CreateAuthorCommand command, CancellationToken token)
    {
        var author = new Author {Id = command.Id, Name = command.Name};

        var exists = dbContext.Authors.Any(e => e.Id == author.Id);
        if (exists) return Errors.Author.DuplicateId;

        dbContext.Authors.Add(author);
        await dbContext.SaveChangesAsync(token);

        // var authorCreatedEvent = new AuthorCreatedEvent
        // {
        //     Id = author.Id,
        //     Name = author.Name
        // };
        // await publisher.Publish(authorCreatedEvent, token);

        return new CreateAuthorResponse(author.Id);
    }
}