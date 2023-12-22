using Application.Data;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authors.EditAuthor;

public class EditAuthorCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<EditAuthorCommand, ErrorOr<EditAuthorResponse>>
{
    public async Task<ErrorOr<EditAuthorResponse>> Handle(EditAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = await dbContext.Authors.FirstOrDefaultAsync(e => e.Id == command.Id, cancellationToken);

        if (author == null) return Errors.Author.NotFound;

        author.Name = command.Name;

        await dbContext.SaveChangesAsync(cancellationToken);
            
        return new EditAuthorResponse(author.Id);
    }
}