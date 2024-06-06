namespace WebApi.Features.Articles.Requests;

public record CreateArticleRequest(string Title, string Content, string AuthorsNote, List<string> CategoryIds);