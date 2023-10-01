using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorWASMCustomAuth.Security.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldToApplicationAuthenticationProviders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ActiveDirectoryFirstLoginAutoRegister",
                table: "ApplicationAuthenticationProviders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveDirectoryFirstLoginAutoRegister",
                table: "ApplicationAuthenticationProviders");
        }
    }
}
