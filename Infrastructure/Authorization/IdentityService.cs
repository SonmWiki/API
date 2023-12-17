using System.Security.Claims;
using Application.Authorization.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

public class IdentityService(IAuthorizationService authorizationService, ICurrentUserService userService)
    : IIdentityService
{
    public string? UserId => userService.UserId;
    public string? UserName => userService.UserName;
    public ClaimsPrincipal? Principal => userService.Principal;

    public async Task<bool> AuthorizeAsync(string policyName)
    {
        return Principal != null && (await authorizationService.AuthorizeAsync(Principal, policyName)).Succeeded;
    }

    public Task<bool> IsInRoleAsync(string role) => Task.FromResult(Principal?.IsInRole(role) ?? false);
    
    public Task<List<string>?> Roles()
    {
        return Task.FromResult(Principal?.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c=>c.Value).ToList() ?? null);
    }
}