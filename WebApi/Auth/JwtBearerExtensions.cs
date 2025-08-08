using System.Security.Claims;
using Application.Data;
using Domain.Entities;
using Domain.Rbac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Auth;

public static class JwtBearerExtensions
{
    public static JwtBearerEvents ConfigureJwtBearerEvents(this JwtBearerOptions options)
    {
        var events = new JwtBearerEvents
        {
            OnTokenValidated = OnTokenValidated,
            OnAuthenticationFailed = OnAuthenticationFailed
        };

        return events;
    }

    private static async Task OnTokenValidated(TokenValidatedContext context)
    {
        var dbContext = context.HttpContext.RequestServices.GetRequiredService<IApplicationDbContext>();
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

        try
        {
            var externalId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cacheKey = $"user_{externalId}";

            if (string.IsNullOrEmpty(externalId))
            {
                logger.LogWarning("Missing sub claim in token");
                context.Fail("Missing sub");
                return;
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.ExternalId == externalId);

            if (user == null)
            {
                var name = context.Principal?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
                if (string.IsNullOrEmpty(name))
                {
                    logger.LogWarning("Missing preferred_username claim for new user");
                    context.Fail("Missing preferred_username");
                    return;
                }

                user = new User
                {
                    Id = default!,
                    ExternalId = externalId,
                    Name = name
                };
                dbContext.Roles.Attach(Roles.Lurker);
                user.Roles.Add(Roles.Lurker);

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
            }

            var claims = new List<Claim>
            {
                new("internal_id", user.Id.ToString())
            };

            var newIdentity = new ClaimsIdentity(claims, context.Scheme.Name);
            context.Principal?.AddIdentity(newIdentity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred on token validation.");
            context.Fail("Authentication error");
        }
    }

    private static Task OnAuthenticationFailed(AuthenticationFailedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(context.Exception, "Authentication failed");
        return Task.CompletedTask;
    }
}