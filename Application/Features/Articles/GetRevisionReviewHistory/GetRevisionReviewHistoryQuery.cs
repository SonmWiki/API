using Application.Common.Messaging;

namespace Application.Features.Articles.GetRevisionReviewHistory;

public record GetRevisionReviewHistoryQuery(Guid Id) : IQuery<GetRevisionReviewHistoryResponse>;