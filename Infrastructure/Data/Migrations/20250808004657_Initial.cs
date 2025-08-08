using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ParentId = table.Column<string>(type: "character varying(512)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Navigations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    Uri = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    Icon = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navigations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Navigations_Navigations_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Navigations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    PermissionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.PermissionsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RedirectArticleId = table.Column<string>(type: "character varying(512)", nullable: true),
                    CurrentRevisionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Articles_RedirectArticleId",
                        column: x => x.RedirectArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryRevision",
                columns: table => new
                {
                    CategoriesId = table.Column<string>(type: "character varying(512)", nullable: false),
                    RevisionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryRevision", x => new { x.CategoriesId, x.RevisionsId });
                    table.ForeignKey(
                        name: "FK_CategoryRevision_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    ReviewTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevisionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Revisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArticleId = table.Column<string>(type: "character varying(512)", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorsNote = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LatestReviewId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Revisions_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Revisions_Reviews_LatestReviewId",
                        column: x => x.LatestReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Revisions_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("27c68a58-6f91-41a2-bd36-c4dda391f309"), "ArticleSeePendingRevisions" },
                    { new Guid("5744b9a3-2256-4f69-b6ac-4d8c164632c9"), "ArticleCreate" },
                    { new Guid("76da2206-a875-47b2-86bb-27ecd0e9f4b9"), "ArticleReviewRevision" },
                    { new Guid("7d9bbc60-f1c8-4762-ac1d-edac85f22b48"), "ArticleSetRedirect" },
                    { new Guid("861b96a8-9a42-45e4-b1c2-733ef85bc2d6"), "ArticleEdit" },
                    { new Guid("95571fee-1313-41de-adfe-6b65fe53760b"), "CategoryCreate" },
                    { new Guid("e4d1aff3-8bc1-4730-b95d-438f1fde6aeb"), "ArticleDelete" },
                    { new Guid("eced433f-b27c-4c6e-af41-d2231fb40f03"), "NavigationsUpdateTree" },
                    { new Guid("fa7c5b48-0fb4-4757-96a7-014b00fdd78b"), "CategoryDelete" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2afacdee-55a4-4e23-9f66-2ca5a5af9751"), "Lurker" },
                    { new Guid("3a7651c5-2cf1-4ed6-9866-c6bac6e8f6dd"), "User" },
                    { new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063"), "Admin" },
                    { new Guid("e1d0ad14-fb96-4488-bf64-8aab2d1ef43d"), "Editor" }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "PermissionsId", "RolesId" },
                values: new object[,]
                {
                    { new Guid("27c68a58-6f91-41a2-bd36-c4dda391f309"), new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063") },
                    { new Guid("27c68a58-6f91-41a2-bd36-c4dda391f309"), new Guid("e1d0ad14-fb96-4488-bf64-8aab2d1ef43d") },
                    { new Guid("5744b9a3-2256-4f69-b6ac-4d8c164632c9"), new Guid("3a7651c5-2cf1-4ed6-9866-c6bac6e8f6dd") },
                    { new Guid("5744b9a3-2256-4f69-b6ac-4d8c164632c9"), new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063") },
                    { new Guid("5744b9a3-2256-4f69-b6ac-4d8c164632c9"), new Guid("e1d0ad14-fb96-4488-bf64-8aab2d1ef43d") },
                    { new Guid("76da2206-a875-47b2-86bb-27ecd0e9f4b9"), new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063") },
                    { new Guid("76da2206-a875-47b2-86bb-27ecd0e9f4b9"), new Guid("e1d0ad14-fb96-4488-bf64-8aab2d1ef43d") },
                    { new Guid("7d9bbc60-f1c8-4762-ac1d-edac85f22b48"), new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063") },
                    { new Guid("7d9bbc60-f1c8-4762-ac1d-edac85f22b48"), new Guid("e1d0ad14-fb96-4488-bf64-8aab2d1ef43d") },
                    { new Guid("861b96a8-9a42-45e4-b1c2-733ef85bc2d6"), new Guid("3a7651c5-2cf1-4ed6-9866-c6bac6e8f6dd") },
                    { new Guid("861b96a8-9a42-45e4-b1c2-733ef85bc2d6"), new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063") },
                    { new Guid("861b96a8-9a42-45e4-b1c2-733ef85bc2d6"), new Guid("e1d0ad14-fb96-4488-bf64-8aab2d1ef43d") },
                    { new Guid("95571fee-1313-41de-adfe-6b65fe53760b"), new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063") },
                    { new Guid("95571fee-1313-41de-adfe-6b65fe53760b"), new Guid("e1d0ad14-fb96-4488-bf64-8aab2d1ef43d") },
                    { new Guid("e4d1aff3-8bc1-4730-b95d-438f1fde6aeb"), new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063") },
                    { new Guid("e4d1aff3-8bc1-4730-b95d-438f1fde6aeb"), new Guid("e1d0ad14-fb96-4488-bf64-8aab2d1ef43d") },
                    { new Guid("eced433f-b27c-4c6e-af41-d2231fb40f03"), new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063") },
                    { new Guid("eced433f-b27c-4c6e-af41-d2231fb40f03"), new Guid("e1d0ad14-fb96-4488-bf64-8aab2d1ef43d") },
                    { new Guid("fa7c5b48-0fb4-4757-96a7-014b00fdd78b"), new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063") },
                    { new Guid("fa7c5b48-0fb4-4757-96a7-014b00fdd78b"), new Guid("e1d0ad14-fb96-4488-bf64-8aab2d1ef43d") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CurrentRevisionId",
                table: "Articles",
                column: "CurrentRevisionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_RedirectArticleId",
                table: "Articles",
                column: "RedirectArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Title",
                table: "Articles",
                column: "Title")
                .Annotation("Npgsql:IndexMethod", "GIST")
                .Annotation("Npgsql:IndexOperators", new[] { "gist_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRevision_RevisionsId",
                table: "CategoryRevision",
                column: "RevisionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Navigations_ParentId",
                table: "Navigations",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerId",
                table: "Reviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RevisionId",
                table: "Reviews",
                column: "RevisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_ArticleId",
                table: "Revisions",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_AuthorId",
                table: "Revisions",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_LatestReviewId",
                table: "Revisions",
                column: "LatestReviewId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RolesId",
                table: "RolePermission",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExternalId",
                table: "Users",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Revisions_CurrentRevisionId",
                table: "Articles",
                column: "CurrentRevisionId",
                principalTable: "Revisions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryRevision_Revisions_RevisionsId",
                table: "CategoryRevision",
                column: "RevisionsId",
                principalTable: "Revisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Revisions_RevisionId",
                table: "Reviews",
                column: "RevisionId",
                principalTable: "Revisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Revisions_CurrentRevisionId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Revisions_RevisionId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "CategoryRevision");

            migrationBuilder.DropTable(
                name: "Navigations");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Revisions");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
