using Domain.Entities;

namespace Application.Authorization.Abstractions;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(Guid userId, Guid permissionId);
    Task<bool> HasPermissionAsync(Guid userId, Permission permission);
    Task<bool> HasAnyPermissionsAsync(Guid userId, IEnumerable<Guid> permissionIds);
    Task<bool> HasAnyPermissionsAsync(Guid userId, IEnumerable<Permission> permissions);
    Task<IEnumerable<Permission>> GetPermissionsASync(Guid userId);
}