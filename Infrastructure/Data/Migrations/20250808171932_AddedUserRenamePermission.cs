using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserRenamePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("d7a48d11-2a3b-4896-8927-e37a6d1d7dd0"), "UserRename" });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "PermissionsId", "RolesId" },
                values: new object[] { new Guid("d7a48d11-2a3b-4896-8927-e37a6d1d7dd0"), new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionsId", "RolesId" },
                keyValues: new object[] { new Guid("d7a48d11-2a3b-4896-8927-e37a6d1d7dd0"), new Guid("ca2cfe04-24ed-42d0-9237-6d5ed7885063") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d7a48d11-2a3b-4896-8927-e37a6d1d7dd0"));
        }
    }
}
