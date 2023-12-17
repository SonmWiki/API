namespace Application.Authorization.Abstractions;

public interface IIdentityService : ICurrentUserService
{
    Task<bool> AuthorizeAsync(string policy);
    Task<bool> IsInRoleAsync(string role);
    Task<List<string>?> Roles();
}