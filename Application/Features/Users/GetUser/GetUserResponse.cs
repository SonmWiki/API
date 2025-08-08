namespace Application.Features.Users.GetUser;

public record GetUserResponse(Guid Id, string Name, IEnumerable<Guid> RoleIds, IEnumerable<Guid> PermissionIds);