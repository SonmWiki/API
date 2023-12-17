using Application.Data;
using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Features.Authors;

public class CreateOrUpdateAuthor
{
    public record Command(string Id, string Name) : IRequest<ErrorOr<Response>>;
    
    public record Response(string Id);
    
    public class CommandHandler(IApplicationDbContext dbContext) : IRequestHandler<Command, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var author = new Author {Id = command.Id, Name = command.Name};
            
            var exists = dbContext.Authors.Any(e => e.Id == author.Id);
            if (exists)
                dbContext.Authors.Update(author);
            else
                dbContext.Authors.Add(author);
            
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return new Response(author.Id);
        }
    }
}