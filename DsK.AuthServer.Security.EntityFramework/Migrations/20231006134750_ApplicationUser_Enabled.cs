﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DsK.AuthServer.Security.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUser_Enabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserEnabled",
                table: "ApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEnabled",
                table: "ApplicationUsers");
        }
    }
}