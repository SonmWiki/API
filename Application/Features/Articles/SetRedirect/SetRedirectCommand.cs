using Application.Common.Messaging;

namespace Application.Features.Articles.SetRedirect;

public record SetRedirectCommand(string ArticleId, string RedirectId) : ICommand<SetRedirectResponse>;