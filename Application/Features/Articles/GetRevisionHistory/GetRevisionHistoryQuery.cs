using Application.Common.Messaging;

namespace Application.Features.Articles.GetRevisionHistory;

public record GetRevisionHistoryQuery(string Id) : IQuery<GetRevisionHistoryResponse>;