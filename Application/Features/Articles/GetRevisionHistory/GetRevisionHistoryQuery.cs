using Application.Common.Messaging;
using ErrorOr;
using MediatR;

namespace Application.Features.Articles.GetRevisionHistory;

public record GetRevisionHistoryQuery(string Id) : IQuery<GetRevisionHistoryResponse>;