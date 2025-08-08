using Application.Common.Messaging;

namespace Application.Features.Users.GetUser;

public record GetUserQuery(Guid Id) : IQuery<GetUserResponse>;