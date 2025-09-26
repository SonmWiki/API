using Domain.Entities;

namespace WebApi.Auth;

public static class MinimalApiExtensions
{
    public static RouteHandlerBuilder RequirePermission(this RouteHandlerBuilder builder, Permission permission)
    {
        return builder.RequireAuthorization($"Permission_{permission.Id:D}");
    }
}