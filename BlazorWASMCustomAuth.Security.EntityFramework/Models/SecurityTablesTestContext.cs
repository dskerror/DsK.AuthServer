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

    public virtual DbSet<AuthenticationProvider> AuthenticationProviders { get; set; }

    public virtual DbSet<AuthenticationProviderAdconfig> AuthenticationProviderAdconfigs { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAuthenticationProvider> UserAuthenticationProviders { get; set; }

    public virtual DbSet<UserAuthenticationProviderDefault> UserAuthenticationProviderDefaults { get; set; }

    public virtual DbSet<UserAuthenticationProviderToken> UserAuthenticationProviderTokens { get; set; }

    public virtual DbSet<UserLog> UserLogs { get; set; }

    public virtual DbSet<UserPassword> UserPasswords { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=SecurityTablesTest;Trusted_Connection=True;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthenticationProvider>(entity =>
        {
            entity.Property(e => e.AuthenticationProviderName).HasMaxLength(50);
        });

        modelBuilder.Entity<AuthenticationProviderAdconfig>(entity =>
        {
            entity.ToTable("AuthenticationProviderADConfig");

            entity.Property(e => e.Domain).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.Property(e => e.PermissionDescription).HasMaxLength(255);
            entity.Property(e => e.PermissionName).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleDescription).HasMaxLength(250);
            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("PK_RolePermissions_1");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Permissions");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissions_Roles");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.LockoutEnd).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserRoles_Roles"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserRoles_Users"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                    });
        });

        modelBuilder.Entity<UserAuthenticationProvider>(entity =>
        {
            entity.HasIndex(e => new { e.UserId, e.AuthenticationProviderId }, "IX_UserAuthenticationProviders").IsUnique();

            entity.Property(e => e.MappedUsername).HasMaxLength(255);

            entity.HasOne(d => d.AuthenticationProvider).WithMany(p => p.UserAuthenticationProviders)
                .HasForeignKey(d => d.AuthenticationProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAuthenticationProviders_AuthenticationProviders");

            entity.HasOne(d => d.User).WithMany(p => p.UserAuthenticationProviders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAuthenticationProviders_Users");
        });

        modelBuilder.Entity<UserAuthenticationProviderDefault>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserAuthenticationProviderDefault");

            entity.Property(e => e.UserId).ValueGeneratedNever();
        });

        modelBuilder.Entity<UserAuthenticationProviderToken>(entity =>
        {
            entity.HasIndex(e => e.UserAuthenticationProviderId, "IX_UserAuthenticationProviderTokens");

            entity.Property(e => e.TokenCreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.TokenRefreshedDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.UserAuthenticationProvider).WithMany(p => p.UserAuthenticationProviderTokens)
                .HasForeignKey(d => d.UserAuthenticationProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAuthenticationProviderTokens_UserAuthenticationProviders");
        });

        modelBuilder.Entity<UserLog>(entity =>
        {
            entity.Property(e => e.EventDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLogs_Users");
        });

        modelBuilder.Entity<UserPassword>(entity =>
        {
            entity.Property(e => e.DateCreated).HasColumnType("date");

            entity.HasOne(d => d.User).WithMany(p => p.UserPasswords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPasswords_Users");
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PermissionId }).HasName("PK_UserPermissions_1");

            entity.HasOne(d => d.Permission).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPermissions_Permissions");

            entity.HasOne(d => d.User).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPermissions_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
