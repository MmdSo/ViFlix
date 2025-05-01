using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViFlix.Data.Migrations
{
    public partial class baseentityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataModified",
                table: "Users",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "DataCreated",
                table: "Users",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "DataModified",
                table: "UserRoles",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "DataCreated",
                table: "UserRoles",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "DataModified",
                table: "Roles",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "DataCreated",
                table: "Roles",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "DataModified",
                table: "RolePermissions",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "DataCreated",
                table: "RolePermissions",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "DataModified",
                table: "Permissions",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "DataCreated",
                table: "Permissions",
                newName: "DateCreated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Users",
                newName: "DataModified");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Users",
                newName: "DataCreated");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "UserRoles",
                newName: "DataModified");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "UserRoles",
                newName: "DataCreated");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Roles",
                newName: "DataModified");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Roles",
                newName: "DataCreated");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "RolePermissions",
                newName: "DataModified");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "RolePermissions",
                newName: "DataCreated");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Permissions",
                newName: "DataModified");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Permissions",
                newName: "DataCreated");
        }
    }
}
