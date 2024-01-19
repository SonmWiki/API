namespace WebApi.Features.Articles.Requests;

public record EditArticleRequest(string Content, List<string> CategoryIds);