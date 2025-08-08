using Domain.Entities;

namespace Application.Authorization.Abstractions;

public interface IUserContext
{
    Guid? UserId { get; }
    Task<User?> GetCurrentUserAsync();
}