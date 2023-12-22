using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedArticleIsHiddenAndRevisionTitleAndCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleStatus",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Revisions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Articles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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
                    table.ForeignKey(
                        name: "FK_CategoryRevision_Revisions_RevisionsId",
                        column: x => x.RevisionsId,
                        principalTable: "Revisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRevision_RevisionsId",
                table: "CategoryRevision",
                column: "RevisionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryRevision");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Revisions");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "ArticleStatus",
                table: "Articles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
