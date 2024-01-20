using Application.Data;
using ErrorOr;
using MediatR;

namespace Application.Features.Articles.DeleteArticle;

public class DeleteArticleCommandHandler
    (IApplicationDbContext dbContext) : IRequestHandler<DeleteArticleCommand, ErrorOr<DeleteArticleResponse>>
{
    public async Task<ErrorOr<DeleteArticleResponse>> Handle(DeleteArticleCommand request, CancellationToken token)
    {
        var article = await dbContext.Articles.FindAsync(new object[] {request.Id}, token);
        if (article == null) return Errors.Article.NotFound;
        dbContext.Articles.Remove(article);
        await dbContext.SaveChangesAsync(token);
        return new DeleteArticleResponse(article.Id);
    }
}