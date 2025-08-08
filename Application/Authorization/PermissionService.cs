using Application.Authorization.Abstractions;
using Application.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Authorization;

public class PermissionService : IPermissionService
{
    private readonly IApplicationDbContext _dbContext;

    public PermissionService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Permission>> GetPermissionsASync(Guid userId)
    {
        return await _dbContext.Users
            .Where(a => a.Id == userId)
            .SelectMany(a => a.Roles)
            .SelectMany(r => r.Permissions)
            .DistinctBy(p => p.Id)
            .ToListAsync();
    }

    public async Task<bool> HasPermissionAsync(Guid userId, Guid permissionId)
    {
        return await _dbContext.Users
            .Where(a => a.Id == userId)
            .SelectMany(a => a.Roles)
            .AnyAsync(r => r.Permissions.Any(p => p.Id == permissionId));
    }

    public async Task<bool> HasPermissionAsync(Guid userId, Permission permission) => await HasPermissionAsync(userId, permission.Id);

    public async Task<bool> HasAnyPermissionsAsync(Guid userId, IEnumerable<Guid> permissionIds)
    {
        return await _dbContext.Users
            .Where(a => a.Id == userId)
            .SelectMany(a => a.Roles)
            .AnyAsync(r => r.Permissions.Any(p => permissionIds.Contains(p.Id)));
    }

    public async Task<bool> HasAnyPermissionsAsync(Guid userId, IEnumerable<Permission> permissions) => await HasAnyPermissionsAsync(userId, permissions.Select(p => p.Id));
}