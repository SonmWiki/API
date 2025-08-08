using Application.Authorization.Abstractions;
using Application.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Auth;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _dbContext;

    public UserContext(IHttpContextAccessor httpContextAccessor, IApplicationDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public Guid? UserId
    {
        get
        {
            var internalId = _httpContextAccessor.HttpContext?.User.FindFirst("internal_id");
            if (internalId == null) return null;
            return Guid.Parse(internalId.Value);
        }
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        if (UserId == null) return null;

        return await _dbContext.Users
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Id == UserId);
    }
}