using Application.Common.Messaging;
using Application.Data;
using Domain.Entities;
using ErrorOr;

namespace Application.Features.Authors.CreateAuthor;

public class CreateAuthorCommandHandler(
    IApplicationDbContext dbContext
) : ICommandHandler<CreateAuthorCommand, CreateAuthorResponse>
{
    public async Task<ErrorOr<CreateAuthorResponse>> HandleAsync(CreateAuthorCommand command, CancellationToken token)
    {
        var author = new Author {Id = command.Id, Name = command.Name};

        var exists = dbContext.Authors.Any(e => e.Id == author.Id);
        if (exists) return Errors.Author.DuplicateId;

        dbContext.Authors.Add(author);
        await dbContext.SaveChangesAsync(token);

        return new CreateAuthorResponse(author.Id);
    }
}