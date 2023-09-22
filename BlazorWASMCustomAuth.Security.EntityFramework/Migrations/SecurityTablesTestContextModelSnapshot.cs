﻿// <auto-generated />
using System;
using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlazorWASMCustomAuth.Security.EntityFramework.Migrations
{
    [DbContext(typeof(SecurityTablesTestContext))]
    partial class SecurityTablesTestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("AppApiKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("ApplicationDesc")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("ApplicationDisabled")
                        .HasColumnType("bit");

                    b.Property<Guid>("ApplicationGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ApplicationGUID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("ApplicationName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CallbackUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("CallbackURL");

                    b.HasKey("Id");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationAuthenticationProvider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ApplicationAuthenticationProviderDisabled")
                        .HasColumnType("bit");

                    b.Property<Guid>("ApplicationAuthenticationProviderGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ApplicationAuthenticationProviderGUID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<string>("AuthenticationProviderType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Domain")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Username")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ApplicationId" }, "IX_ApplicationAuthenticationProviders_ApplicationId");

                    b.HasIndex(new[] { "AuthenticationProviderType" }, "IX_ApplicationAuthenticationProviders_AuthenticationProviderId");

                    b.ToTable("ApplicationAuthenticationProviders");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationAuthenticationProviderLogin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationAuthenticationProviderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTimeGenerated")
                        .HasColumnType("datetime");

                    b.Property<Guid>("LoginKey")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ApplicationAuthenticationProviderId" }, "IX_ApplicationAuthenticationProviderLogins_ApplicationAuthenticationProviderId");

                    b.ToTable("ApplicationAuthenticationProviderLogins");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<string>("PermissionDescription")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("PermissionDisabled")
                        .HasColumnType("bit");

                    b.Property<string>("PermissionName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK_Permissions");

                    b.HasIndex(new[] { "ApplicationId" }, "IX_ApplicationPermissions_ApplicationId");

                    b.HasIndex(new[] { "PermissionName", "ApplicationId" }, "IX_Permissions")
                        .IsUnique();

                    b.ToTable("ApplicationPermissions");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<string>("RoleDescription")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("RoleDisabled")
                        .HasColumnType("bit");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK_Roles");

                    b.HasIndex(new[] { "ApplicationId" }, "IX_ApplicationRoles_ApplicationId");

                    b.HasIndex(new[] { "RoleName", "ApplicationId" }, "IX_Roles")
                        .IsUnique();

                    b.ToTable("ApplicationRoles");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationRolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "PermissionId")
                        .HasName("PK_RolePermissions_1");

                    b.ToTable("ApplicationRolePermissions");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LockoutEnd")
                        .HasColumnType("date");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ApplicationId" }, "IX_ApplicationUsers_ApplicationId");

                    b.HasIndex(new[] { "UserId" }, "IX_ApplicationUsers_UserId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LockoutEnd")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Email" }, "IX_Users_Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserAuthenticationProviderMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationAuthenticationProviderId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id")
                        .HasName("PK_UserAuthenticationProviders");

                    b.HasIndex("ApplicationAuthenticationProviderId");

                    b.HasIndex(new[] { "UserId" }, "IX_UserAuthenticationProviders_UserId");

                    b.ToTable("UserAuthenticationProviderMappings");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("IP");

                    b.Property<DateTime>("LogDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QueryString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ApplicationId" }, "IX_UserLogs_ApplicationId");

                    b.HasIndex(new[] { "UserId" }, "IX_UserLogs_UserId");

                    b.ToTable("UserLogs");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserPassword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId" }, "IX_UserPasswords_UserId");

                    b.ToTable("UserPasswords");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Allow")
                        .HasColumnType("bit");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "PermissionId", "UserId" }, "IX_UserPermissions")
                        .IsUnique();

                    b.HasIndex(new[] { "UserId" }, "IX_UserPermissions_UserId");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId", "RoleId" }, "IX_UserRoles")
                        .IsUnique();

                    b.HasIndex(new[] { "RoleId" }, "IX_UserRoles_RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TokenCreatedDateTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("TokenRefreshedDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_UserAuthenticationProviderTokens");

                    b.HasIndex(new[] { "ApplicationId" }, "IX_UserTokens_ApplicationId");

                    b.HasIndex(new[] { "UserId" }, "IX_UserTokens_UserId");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationAuthenticationProvider", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.Application", "Application")
                        .WithMany("ApplicationAuthenticationProviders")
                        .HasForeignKey("ApplicationId")
                        .IsRequired()
                        .HasConstraintName("FK_ApplicationAuthenticationProviders_Applications");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationAuthenticationProviderLogin", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationAuthenticationProvider", "ApplicationAuthenticationProvider")
                        .WithMany("ApplicationAuthenticationProviderLogins")
                        .HasForeignKey("ApplicationAuthenticationProviderId")
                        .IsRequired()
                        .HasConstraintName("FK_ApplicationAuthenticationProviderLogins_ApplicationAuthenticationProviders");

                    b.Navigation("ApplicationAuthenticationProvider");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationPermission", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.Application", "Application")
                        .WithMany("ApplicationPermissions")
                        .HasForeignKey("ApplicationId")
                        .IsRequired()
                        .HasConstraintName("FK_ApplicationPermissions_Applications");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationRole", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.Application", "Application")
                        .WithMany("ApplicationRoles")
                        .HasForeignKey("ApplicationId")
                        .IsRequired()
                        .HasConstraintName("FK_ApplicationRoles_Applications");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationRolePermission", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationPermission", "Role")
                        .WithMany("ApplicationRolePermissions")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK_RolePermissions_Permissions");

                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationRole", "RoleNavigation")
                        .WithMany("ApplicationRolePermissions")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK_RolePermissions_Roles");

                    b.Navigation("Role");

                    b.Navigation("RoleNavigation");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationUser", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.Application", "Application")
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("ApplicationId")
                        .IsRequired()
                        .HasConstraintName("FK_ApplicationUsers_Applications");

                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.User", "User")
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_ApplicationUsers_Users");

                    b.Navigation("Application");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserAuthenticationProviderMapping", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationAuthenticationProvider", "ApplicationAuthenticationProvider")
                        .WithMany("UserAuthenticationProviderMappings")
                        .HasForeignKey("ApplicationAuthenticationProviderId")
                        .IsRequired()
                        .HasConstraintName("FK_UserAuthenticationProviderMappings_ApplicationAuthenticationProviders");

                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.User", "User")
                        .WithMany("UserAuthenticationProviderMappings")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UserAuthenticationProviders_Users");

                    b.Navigation("ApplicationAuthenticationProvider");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserLog", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.Application", "Application")
                        .WithMany("UserLogs")
                        .HasForeignKey("ApplicationId")
                        .IsRequired()
                        .HasConstraintName("FK_UserLogs_Users");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserPassword", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.User", "User")
                        .WithMany("UserPasswords")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UserPasswords_Users");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserPermission", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationPermission", "Permission")
                        .WithMany("UserPermissions")
                        .HasForeignKey("PermissionId")
                        .IsRequired()
                        .HasConstraintName("FK_UserPermissions_Permissions");

                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.User", "User")
                        .WithMany("UserPermissions")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UserPermissions_Users");

                    b.Navigation("Permission");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserRole", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK_UserRoles_Roles");

                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UserRoles_Users");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.UserToken", b =>
                {
                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.Application", "Application")
                        .WithMany("UserTokens")
                        .HasForeignKey("ApplicationId")
                        .HasConstraintName("FK_UserTokens_Applications");

                    b.HasOne("BlazorWASMCustomAuth.Security.EntityFramework.Models.User", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UserTokens_Users");

                    b.Navigation("Application");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.Application", b =>
                {
                    b.Navigation("ApplicationAuthenticationProviders");

                    b.Navigation("ApplicationPermissions");

                    b.Navigation("ApplicationRoles");

                    b.Navigation("ApplicationUsers");

                    b.Navigation("UserLogs");

                    b.Navigation("UserTokens");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationAuthenticationProvider", b =>
                {
                    b.Navigation("ApplicationAuthenticationProviderLogins");

                    b.Navigation("UserAuthenticationProviderMappings");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationPermission", b =>
                {
                    b.Navigation("ApplicationRolePermissions");

                    b.Navigation("UserPermissions");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.ApplicationRole", b =>
                {
                    b.Navigation("ApplicationRolePermissions");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("BlazorWASMCustomAuth.Security.EntityFramework.Models.User", b =>
                {
                    b.Navigation("ApplicationUsers");

                    b.Navigation("UserAuthenticationProviderMappings");

                    b.Navigation("UserPasswords");

                    b.Navigation("UserPermissions");

                    b.Navigation("UserRoles");

                    b.Navigation("UserTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
