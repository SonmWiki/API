using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TableRelationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Articles_Articleid",
                table: "Revisions");

            migrationBuilder.AlterColumn<int>(
                name: "Articleid",
                table: "Revisions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Articles_Articleid",
                table: "Revisions",
                column: "Articleid",
                principalTable: "Articles",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Articles_Articleid",
                table: "Revisions");

            migrationBuilder.AlterColumn<int>(
                name: "Articleid",
                table: "Revisions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Articles_Articleid",
                table: "Revisions",
                column: "Articleid",
                principalTable: "Articles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
