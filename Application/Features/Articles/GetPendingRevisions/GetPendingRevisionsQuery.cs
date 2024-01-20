using ErrorOr;
using MediatR;

namespace Application.Features.Articles.GetPendingRevisions;

public record GetPendingRevisionsQuery : IRequest<ErrorOr<GetPendingRevisionsResponse>>;