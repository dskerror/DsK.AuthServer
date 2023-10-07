using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorWASMCustomAuth.Security.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Allow",
                table: "UserPermissions",
                newName: "Overwrite");

            migrationBuilder.RenameColumn(
                name: "ApplicationDisabled",
                table: "Applications",
                newName: "IsEnabled");

            migrationBuilder.RenameColumn(
                name: "RoleDisabled",
                table: "ApplicationRoles",
                newName: "IsEnabled");

            migrationBuilder.RenameColumn(
                name: "PermissionDisabled",
                table: "ApplicationPermissions",
                newName: "IsEnabled");

            migrationBuilder.RenameColumn(
                name: "ApplicationAuthenticationProviderDisabled",
                table: "ApplicationAuthenticationProviders",
                newName: "IsEnabled");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "ApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "ApplicationAuthenticationProviderUserMappings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "ApplicationAuthenticationProviderUserMappings");

            migrationBuilder.RenameColumn(
                name: "Overwrite",
                table: "UserPermissions",
                newName: "Allow");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "Applications",
                newName: "ApplicationDisabled");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "ApplicationRoles",
                newName: "RoleDisabled");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "ApplicationPermissions",
                newName: "PermissionDisabled");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "ApplicationAuthenticationProviders",
                newName: "ApplicationAuthenticationProviderDisabled");
        }
    }
}
