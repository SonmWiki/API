using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedArticleRedirectArticleOnDeleteBehaviorToCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Articles_RedirectArticleId",
                table: "Articles");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Articles_RedirectArticleId",
                table: "Articles",
                column: "RedirectArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Articles_RedirectArticleId",
                table: "Articles");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Articles_RedirectArticleId",
                table: "Articles",
                column: "RedirectArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }
    }
}
