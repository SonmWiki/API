using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Features.Articles.SetRedirect;

public record SetRedirectCommand(string ArticleId, string RedirectId) : IRequest<ErrorOr<SetRedirectResponse>>;