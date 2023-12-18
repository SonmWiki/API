using Application.Data;
using Application.Features.Authors.CreateAuthor;
using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Features.Authors.UpdateAuthor;

public class UpdateAuthorCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateAuthorCommand, ErrorOr<CreateAuthorResponse>>
{
    public async Task<ErrorOr<CreateAuthorResponse>> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = new Author {Id = command.Id, Name = command.Name};
            
        var exists = dbContext.Authors.Any(e => e.Id == author.Id);
        if (!exists) return Errors.Author.NotFound;

        dbContext.Authors.Update(author);
        await dbContext.SaveChangesAsync(cancellationToken);
            
        return new CreateAuthorResponse(author.Id);
    }
}