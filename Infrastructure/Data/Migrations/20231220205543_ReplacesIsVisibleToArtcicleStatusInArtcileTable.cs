using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplacesIsVisibleToArtcicleStatusInArtcileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "ArticleStatus",
                table: "Articles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleStatus",
                table: "Articles");

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Articles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
