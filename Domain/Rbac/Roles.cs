using Domain.Entities;

namespace Domain.Rbac;

public static class Roles
{
    public static Role Admin { get; } = new()
    {
        Id = new Guid("CA2CFE04-24ED-42D0-9237-6D5ED7885063"),
        Name = "Admin",
    };

    public static Role Editor { get; } = new()
    {
        Id = new Guid("E1D0AD14-FB96-4488-BF64-8AAB2D1EF43D"),
        Name = "Editor",
    };

    public static Role User { get; } = new()
    {
        Id = new Guid("3A7651C5-2CF1-4ED6-9866-C6BAC6E8F6DD"),
        Name = "User",
    };

    public static Role Lurker { get; } = new()
    {
        Id = new Guid("2AFACDEE-55A4-4E23-9F66-2CA5A5AF9751"),
        Name = "Lurker",
    };

    public static IReadOnlyCollection<Role> All => [Admin, Editor, User, Lurker];

    public static IReadOnlyCollection<Permission> GetDefaultPermissions(Role role) => role.Id switch
    {
        _ when role.Id == Admin.Id => AdminPermissions,
        _ when role.Id == Editor.Id => EditorPermissions,
        _ when role.Id == User.Id => UserPermissions,
        _ when role.Id == Lurker.Id => LurkerPermissions,
        _ => []
    };

    private static readonly IReadOnlyCollection<Permission> AdminPermissions = new List<Permission>
    {
        Permissions.ArticleCreate,
        Permissions.ArticleDelete,
        Permissions.ArticleEdit,
        Permissions.ArticleSeePendingRevisions,
        Permissions.ArticleReviewRevision,
        Permissions.ArticleSetRedirect,
        Permissions.CategoryCreate,
        Permissions.CategoryDelete,
        Permissions.NavigationsUpdateTree,
    };

    private static readonly IReadOnlyCollection<Permission> EditorPermissions = new List<Permission>
    {
        Permissions.ArticleCreate,
        Permissions.ArticleDelete,
        Permissions.ArticleEdit,
        Permissions.ArticleSeePendingRevisions,
        Permissions.ArticleReviewRevision,
        Permissions.ArticleSetRedirect,
        Permissions.CategoryCreate,
        Permissions.CategoryDelete,
        Permissions.NavigationsUpdateTree,
    };

    private static readonly IReadOnlyCollection<Permission> UserPermissions = new List<Permission>
    {
        Permissions.ArticleCreate,
        Permissions.ArticleEdit
    };

    private static readonly IReadOnlyCollection<Permission> LurkerPermissions = new List<Permission>
    {
    };
}