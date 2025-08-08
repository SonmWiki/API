using Application.Common.Messaging;
using Application.Data;
using Application.Features.Users.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.GetUser;

public class GetUserQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetUserQuery, GetUserResponse>
{
    public async Task<ErrorOr<GetUserResponse>> HandleAsync(GetUserQuery query, CancellationToken token)
    {
        var user = await dbContext.Users
            .Include(u => u.Roles)
            .ThenInclude(u => u.Permissions)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == query.Id, token);

        if (user == null) return User.NotFound;

        var roleIds = user.Roles.Select(r => r.Id).ToList();

        var permissionIds = user.Roles
            .SelectMany(r => r.Permissions)
            .Select(p => p.Id)
            .Distinct()
            .ToList();

        return new GetUserResponse(
            user.Id,
            user.Name,
            roleIds,
            permissionIds
        );
    }
}