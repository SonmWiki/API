using Application.Common.Messaging;
using ErrorOr;
using MediatR;

namespace Application.Features.Articles.GetRevisionReviewHistory;

public record GetRevisionReviewHistoryQuery(Guid Id) : IQuery<GetRevisionReviewHistoryResponse>;