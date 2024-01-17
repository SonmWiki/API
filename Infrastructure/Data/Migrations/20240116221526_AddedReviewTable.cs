using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Review",
                table: "Revisions");

            migrationBuilder.DropColumn(
                name: "ReviewTimestamp",
                table: "Revisions");

            migrationBuilder.DropColumn(
                name: "Reviewer",
                table: "Revisions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Revisions");

            migrationBuilder.AddColumn<Guid>(
                name: "LatestReviewId",
                table: "Revisions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    ReviewTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevisionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Authors_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Revisions_RevisionId",
                        column: x => x.RevisionId,
                        principalTable: "Revisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_LatestReviewId",
                table: "Revisions",
                column: "LatestReviewId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerId",
                table: "Reviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RevisionId",
                table: "Reviews",
                column: "RevisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Reviews_LatestReviewId",
                table: "Revisions",
                column: "LatestReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Reviews_LatestReviewId",
                table: "Revisions");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Revisions_LatestReviewId",
                table: "Revisions");

            migrationBuilder.DropColumn(
                name: "LatestReviewId",
                table: "Revisions");

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Revisions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewTimestamp",
                table: "Revisions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reviewer",
                table: "Revisions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Revisions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
