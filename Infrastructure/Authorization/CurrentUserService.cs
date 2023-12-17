using System.Security.Claims;
using Application.Authorization.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authorization;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId => httpContextAccessor
        .HttpContext
        ?.User
        .FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserName => httpContextAccessor
        .HttpContext
        ?.User
        .FindFirstValue("preferred_username");

    public ClaimsPrincipal? Principal => httpContextAccessor
        .HttpContext
        ?.User;
}