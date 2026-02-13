using Domain.Entities;

namespace WebApi.Auth;

public static class MinimalApiExtensions
{
    extension(RouteHandlerBuilder builder)
    {
        public RouteHandlerBuilder RequirePermission(Permission permission)
        {
            return builder.RequireAuthorization($"Permission_{permission.Id:D}");
        }
    }
}