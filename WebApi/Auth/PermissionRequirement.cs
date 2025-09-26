using System.Security.Claims;
using Application.Authorization.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Auth;

public class PermissionRequirement : IAuthorizationRequirement
{
    public Permission[] Permissions { get; }

    public PermissionRequirement(params Permission[] permissions)
    {
        Permissions = permissions;
    }
}

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserContext _userContext;

    public PermissionAuthorizationHandler(IUserContext userContext)
    {
        _userContext = userContext;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var user = await _userContext.GetCurrentUserAsync();
        if (user == null)
        {
            return;
        }

        var hasAnyPermission = user.Roles
            .Any(r => r.Permissions.Any(p => requirement.Permissions.Select(x => x.Id).Contains(p.Id)));

        if (hasAnyPermission)
        {
            context.Succeed(requirement);
        }
    }
}