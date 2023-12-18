namespace WebApi.Features.Articles.Requests;

public record CreateArticleRequest(string Title, string Content, List<string> CategoryIds);