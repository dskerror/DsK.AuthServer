using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorWASMCustomAuth.Security.EntityFramework.Models;

public partial class SecurityTablesTestContext : DbContext
{
    public SecurityTablesTestContext()
    {
    }

    public SecurityTablesTestContext(DbContextOptions<SecurityTablesTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<ApplicationAuthenticationProvider> ApplicationAuthenticationProviders { get; set; }

    public virtual DbSet<ApplicationPermission> ApplicationPermissions { get; set; }

    public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }

    public virtual DbSet<ApplicationRolePermission> ApplicationRolePermissions { get; set; }

    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public virtual DbSet<AuthenticationProvider> AuthenticationProviders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAuthenticationProviderMapping> UserAuthenticationProviderMappings { get; set; }

    public virtual DbSet<UserLog> UserLogs { get; set; }

    public virtual DbSet<UserPassword> UserPasswords { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=SecurityTablesTest;Trusted_Connection=True;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.Property(e => e.AppApiKey).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ApplicationDesc).HasMaxLength(250);
            entity.Property(e => e.ApplicationGuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ApplicationGUID");
            entity.Property(e => e.ApplicationName).HasMaxLength(50);
        });

        modelBuilder.Entity<ApplicationAuthenticationProvider>(entity =>
        {
            entity.HasIndex(e => e.ApplicationId, "IX_ApplicationAuthenticationProviders_ApplicationId");

            entity.HasIndex(e => e.AuthenticationProviderId, "IX_ApplicationAuthenticationProviders_AuthenticationProviderId");

            entity.Property(e => e.Domain).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.Application).WithMany(p => p.ApplicationAuthenticationProviders)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationAuthenticationProviders_Applications");

            entity.HasOne(d => d.AuthenticationProvider).WithMany(p => p.ApplicationAuthenticationProviders)
                .HasForeignKey(d => d.AuthenticationProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationAuthenticationProviders_AuthenticationProviders");
        });

        modelBuilder.Entity<ApplicationPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Permissions");

            entity.HasIndex(e => e.ApplicationId, "IX_ApplicationPermissions_ApplicationId");

            entity.HasIndex(e => e.PermissionName, "IX_Permissions").IsUnique();

            entity.Property(e => e.PermissionDescription).HasMaxLength(250);
            entity.Property(e => e.PermissionName).HasMaxLength(50);

            entity.HasOne(d => d.Application).WithMany(p => p.ApplicationPermissions)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationPermissions_Applications");
        });

        modelBuilder.Entity<ApplicationRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Roles");

            entity.HasIndex(e => e.ApplicationId, "IX_ApplicationRoles_ApplicationId");

            entity.HasIndex(e => e.RoleName, "IX_Roles").IsUnique();

            entity.Property(e => e.RoleDescription).HasMaxLength(250);
            entity.Property(e => e.RoleName).HasMaxLength(50);

            entity.HasOne(d => d.Application).WithMany(p => p.ApplicationRoles)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationRoles_Applications");
        });

        modelBuilder.Entity<ApplicationRolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("PK_RolePermissions_1");

            entity.HasOne(d => d.Role).WithMany(p => p.ApplicationRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Permissions");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.ApplicationRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Roles");
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.LockoutEnd).HasColumnType("date");

            entity.HasOne(d => d.Application).WithMany(p => p.ApplicationUsers)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationUsers_Applications");

            entity.HasOne(d => d.User).WithMany(p => p.ApplicationUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationUsers_Users");
        });

        modelBuilder.Entity<AuthenticationProvider>(entity =>
        {
            entity.HasIndex(e => e.AuthenticationProviderName, "IX_AuthenticationProviders");

            entity.Property(e => e.AuthenticationProviderName).HasMaxLength(50);
            entity.Property(e => e.AuthenticationProviderType).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.LockoutEnd).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<UserAuthenticationProviderMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserAuthenticationProviders");

            entity.HasIndex(e => e.ApplicationAuthenticationProviderId, "IX_UserAuthenticationProviders_AuthenticationProviderId");

            entity.HasIndex(e => e.UserId, "IX_UserAuthenticationProviders_UserId");

            entity.Property(e => e.Username).HasMaxLength(256);

            entity.HasOne(d => d.ApplicationAuthenticationProvider).WithMany(p => p.UserAuthenticationProviderMappings)
                .HasForeignKey(d => d.ApplicationAuthenticationProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAuthenticationProviders_ApplicationAuthenticationProviders");

            entity.HasOne(d => d.User).WithMany(p => p.UserAuthenticationProviderMappings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAuthenticationProviders_Users");
        });

        modelBuilder.Entity<UserLog>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_UserLogs_UserId");

            entity.Property(e => e.Ip).HasColumnName("IP");
            entity.Property(e => e.LogDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Application).WithMany(p => p.UserLogs)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLogs_Users");
        });

        modelBuilder.Entity<UserPassword>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_UserPasswords_UserId");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.UserPasswords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPasswords_Users");
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasIndex(e => new { e.PermissionId, e.UserId }, "IX_UserPermissions").IsUnique();

            entity.HasIndex(e => e.UserId, "IX_UserPermissions_UserId");

            entity.HasOne(d => d.Permission).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPermissions_Permissions");

            entity.HasOne(d => d.User).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPermissions_Users");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasIndex(e => new { e.UserId, e.RoleId }, "IX_UserRoles").IsUnique();

            entity.HasIndex(e => e.RoleId, "IX_UserRoles_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Roles");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Users");
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserAuthenticationProviderTokens");

            entity.HasIndex(e => e.ApplicationId, "IX_UserTokens_ApplicationId");

            entity.HasIndex(e => e.UserId, "IX_UserTokens_UserId");

            entity.Property(e => e.TokenCreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.TokenRefreshedDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Application).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.ApplicationId)
                .HasConstraintName("FK_UserTokens_Applications");

            entity.HasOne(d => d.User).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserTokens_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
