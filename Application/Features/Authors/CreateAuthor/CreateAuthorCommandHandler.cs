using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Features.Authors.CreateAuthor;

public class CreateAuthorCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateAuthorCommand, ErrorOr<CreateAuthorResponse>>
{
    public async Task<ErrorOr<CreateAuthorResponse>> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = new Author {Id = command.Id, Name = command.Name};
            
        var exists = dbContext.Authors.Any(e => e.Id == author.Id);
        if (exists) return Errors.Author.DuplicateId;

        dbContext.Authors.Add(author);
        await dbContext.SaveChangesAsync(cancellationToken);
            
        return new CreateAuthorResponse(author.Id);
    }
}