using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedWrongForeignKeyNameInNavigations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Navigations_Navigations_NavigationId",
                table: "Navigations");

            migrationBuilder.DropIndex(
                name: "IX_Navigations_NavigationId",
                table: "Navigations");

            migrationBuilder.DropColumn(
                name: "NavigationId",
                table: "Navigations");

            migrationBuilder.CreateIndex(
                name: "IX_Navigations_ParentId",
                table: "Navigations",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Navigations_Navigations_ParentId",
                table: "Navigations",
                column: "ParentId",
                principalTable: "Navigations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Navigations_Navigations_ParentId",
                table: "Navigations");

            migrationBuilder.DropIndex(
                name: "IX_Navigations_ParentId",
                table: "Navigations");

            migrationBuilder.AddColumn<int>(
                name: "NavigationId",
                table: "Navigations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Navigations_NavigationId",
                table: "Navigations",
                column: "NavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Navigations_Navigations_NavigationId",
                table: "Navigations",
                column: "NavigationId",
                principalTable: "Navigations",
                principalColumn: "Id");
        }
    }
}
