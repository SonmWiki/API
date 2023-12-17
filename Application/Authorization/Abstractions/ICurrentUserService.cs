using System.Security.Claims;

namespace Application.Authorization.Abstractions;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    ClaimsPrincipal? Principal { get; }
}