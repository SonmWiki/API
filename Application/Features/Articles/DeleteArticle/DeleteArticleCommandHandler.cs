using Application.Data;
using ErrorOr;

namespace Application.Features.Articles.DeleteArticle;

public class DeleteArticleCommandHandler(IApplicationDbContext dbContext) : IDeleteArticleCommandHandler
{
    public async Task<ErrorOr<DeleteArticleResponse>> Handle(DeleteArticleCommand request, CancellationToken token)
    {
        var article = await dbContext.Articles.FindAsync(new object[] {request.Id}, token);
        if (article == null) return Errors.Article.NotFound;
        dbContext.Articles.Remove(article);
        await dbContext.SaveChangesAsync(token);

        // var articleDeletedEvent = new ArticleDeletedEvent {Id = article.Id};
        // await publisher.Publish(articleDeletedEvent, token);

        return new DeleteArticleResponse(article.Id);
    }
}