using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Features.Authors.UpdateAuthor;

public class UpdateAuthorCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateAuthorCommand, ErrorOr<UpdateAuthorResponse>>
{
    public async Task<ErrorOr<UpdateAuthorResponse>> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = new Author {Id = command.Id, Name = command.Name};
            
        var exists = dbContext.Authors.Any(e => e.Id == author.Id);
        if (!exists) return Errors.Author.NotFound;

        dbContext.Authors.Update(author);
        await dbContext.SaveChangesAsync(cancellationToken);
            
        return new UpdateAuthorResponse(author.Id);
    }
}