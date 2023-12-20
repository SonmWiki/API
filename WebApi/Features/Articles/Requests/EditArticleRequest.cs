namespace WebApi.Features.Articles.Requests;

public record EditArticleRequest(string Title, string Content, List<string> CategoryIds);