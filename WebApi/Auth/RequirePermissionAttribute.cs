using Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Auth;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(Permission permission) : base(policy: $"Permission_{permission.Id}")
    {
        Policy = $"Permission_{permission.Id}";
    }
}