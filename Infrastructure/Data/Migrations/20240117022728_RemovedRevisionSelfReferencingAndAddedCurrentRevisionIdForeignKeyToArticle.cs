using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRevisionSelfReferencingAndAddedCurrentRevisionIdForeignKeyToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Revisions_PreviousRevisionId",
                table: "Revisions");

            migrationBuilder.DropIndex(
                name: "IX_Revisions_PreviousRevisionId",
                table: "Revisions");

            migrationBuilder.DropColumn(
                name: "PreviousRevisionId",
                table: "Revisions");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentRevisionId",
                table: "Articles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CurrentRevisionId",
                table: "Articles",
                column: "CurrentRevisionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Revisions_CurrentRevisionId",
                table: "Articles",
                column: "CurrentRevisionId",
                principalTable: "Revisions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Revisions_CurrentRevisionId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_CurrentRevisionId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CurrentRevisionId",
                table: "Articles");

            migrationBuilder.AddColumn<Guid>(
                name: "PreviousRevisionId",
                table: "Revisions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_PreviousRevisionId",
                table: "Revisions",
                column: "PreviousRevisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Revisions_PreviousRevisionId",
                table: "Revisions",
                column: "PreviousRevisionId",
                principalTable: "Revisions",
                principalColumn: "Id");
        }
    }
}
