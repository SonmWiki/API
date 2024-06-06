namespace WebApi.Features.Articles.Requests;

public record EditArticleRequest(string Content, string AuthorsNote, List<string> CategoryIds);