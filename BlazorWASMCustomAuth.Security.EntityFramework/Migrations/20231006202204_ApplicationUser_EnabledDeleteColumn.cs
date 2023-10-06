using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorWASMCustomAuth.Security.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUser_EnabledDeleteColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEnabled",
                table: "ApplicationUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserEnabled",
                table: "ApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
