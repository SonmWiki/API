using ErrorOr;
using MediatR;

namespace Application.Features.Articles.GetRevisionHistory;

public record GetRevisionHistoryQuery(string Id) : IRequest<ErrorOr<GetRevisionHistoryResponse>>;