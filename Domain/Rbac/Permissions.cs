using Domain.Entities;

namespace Domain.Rbac;

public static class Permissions
{
    public static Permission ArticleCreate = new()
    {
        Id = new Guid("5744B9A3-2256-4F69-B6AC-4D8C164632C9"),
        Name = nameof(ArticleCreate),
    };

    public static Permission ArticleEdit = new()
    {
        Id = new Guid("861B96A8-9A42-45E4-B1C2-733EF85BC2D6"),
        Name = nameof(ArticleEdit),
    };

    public static Permission ArticleSetRedirect = new()
    {
        Id = new Guid("7D9BBC60-F1C8-4762-AC1D-EDAC85F22B48"),
        Name = nameof(ArticleSetRedirect),
    };

    public static Permission ArticleSeePendingRevisions = new()
    {
        Id = new Guid("27C68A58-6F91-41A2-BD36-C4DDA391F309"),
        Name = nameof(ArticleSeePendingRevisions),
    };

    public static Permission ArticleReviewRevision = new()
    {
        Id = new Guid("76DA2206-A875-47B2-86BB-27ECD0E9F4B9"),
        Name = nameof(ArticleReviewRevision),
    };

    public static Permission ArticleDelete = new()
    {
        Id = new Guid("E4D1AFF3-8BC1-4730-B95D-438F1FDE6AEB"),
        Name = nameof(ArticleDelete),
    };

    public static Permission CategoryCreate = new()
    {
        Id = new Guid("95571FEE-1313-41DE-ADFE-6B65FE53760B"),
        Name = nameof(CategoryCreate),
    };

    public static Permission CategoryDelete = new()
    {
        Id = new Guid("FA7C5B48-0FB4-4757-96A7-014B00FDD78B"),
        Name = nameof(CategoryDelete),
    };

    public static Permission NavigationsUpdateTree = new()
    {
        Id = new Guid("ECED433F-B27C-4C6E-AF41-D2231FB40F03"),
        Name = nameof(NavigationsUpdateTree),
    };

    public static IReadOnlyCollection<Permission> All =>
    [
        ArticleCreate,
        ArticleEdit,
        ArticleSetRedirect,
        ArticleSeePendingRevisions,
        ArticleReviewRevision,
        ArticleDelete,
        CategoryCreate,
        CategoryDelete,
        NavigationsUpdateTree
    ];
}