using Application.Common.Messaging;

namespace Application.Features.Articles.DeleteArticle;

public record DeleteArticleCommand(string Id) : ICommand<DeleteArticleResponse>;