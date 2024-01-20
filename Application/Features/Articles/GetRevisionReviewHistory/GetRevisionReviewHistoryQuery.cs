using ErrorOr;
using MediatR;

namespace Application.Features.Articles.GetRevisionReviewHistory;

public record GetRevisionReviewHistoryQuery(Guid Id) : IRequest<ErrorOr<GetRevisionReviewHistoryResponse>>;