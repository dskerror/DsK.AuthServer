﻿using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

internal class Program
{
    private static void Main(string[] args)
    {
        var options = new DbContextOptions<SecurityTablesTestContext>();

        //var db = new SecurityTablesTestContext(new DbContextOptionsBuilder<SecurityTablesTestContext>().UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SecurityTablesTest;Trusted_Connection=True;Trust Server Certificate=true").Options);
        var db = new SecurityTablesTestContext(new DbContextOptionsBuilder<SecurityTablesTestContext>().UseSqlServer("Server=.;Database=SecurityTablesTest;Trusted_Connection=True;Trust Server Certificate=true").Options);
        db.Database.Migrate(); //CREATES DATABASE IF IT DOESNT EXISTS
        db.Database.EnsureCreated(); //CREATES TABLES IF IT DOESNT EXISTS

        Application newApplication = CreateApplication(db);
        AuthenticationProvider localAuthProvider = CreateLocalAuthProvider(db);
        CreateApplicationAuthenticationUserProvider(db, newApplication, localAuthProvider);
        ApplicationPermission adminPermission = CreateAdminPermission(db);
        ApplicationRole adminRole = CreateAdminRole(db);
        CreateUserRole(db);
        AddAdminPermissionToAdminRole(db, adminPermission, adminRole);
        User adminUser = CreateAdminUser(db);
        AddAuthenticationProviderMappingToAdminUser(db, localAuthProvider, adminUser);
        AddAdminRoleToAdminUser(db, adminRole, adminUser);

        CreateAdminUserPassword(db, adminUser);

        var permissionList = BlazorWASMCustomAuth.Security.Shared.Constants.Access.GetRegisteredPermissions();
        foreach (var permission in permissionList)
        {
            db.ApplicationPermissions.Add(new ApplicationPermission() { PermissionName = permission, PermissionDescription = "" });
        }
        db.SaveChanges();

    }

    private static void CreateApplicationAuthenticationUserProvider(SecurityTablesTestContext db, Application newApplication, AuthenticationProvider localAuthProvider)
    {
        ApplicationAuthenticationProvider applicationAuthenticationProvider =
                new ApplicationAuthenticationProvider()
                {
                    ApplicationId = newApplication.Id,
                    AuthenticationProviderId = localAuthProvider.Id,
                    Domain = "",
                    Username = "",
                    Password = ""
                };

        db.ApplicationAuthenticationProviders.Add(applicationAuthenticationProvider);
        db.SaveChanges();
    }

    private static void CreateAdminUserPassword(SecurityTablesTestContext db, User adminUser)
    {
        var ramdomSalt = SecurityHelpers.RandomizeSalt;

        var userPassword = new UserPassword()
        {
            UserId = adminUser.Id,
            HashedPassword = SecurityHelpers.HashPasword("admin123", ramdomSalt),
            Salt = Convert.ToHexString(ramdomSalt),
            DateCreated = DateTime.Now
        };
        db.UserPasswords.Add(userPassword);
        db.SaveChanges();
    }

    private static Application CreateApplication(SecurityTablesTestContext db)
    {
        Application newApplication = new Application()
        {
            ApplicationName = "DsK.Scurity",
            ApplicationDesc = "Manages security for other applications"
        };

        db.Applications.Add(newApplication);
        db.SaveChanges();
        return newApplication;
    }
    private static AuthenticationProvider CreateLocalAuthProvider(SecurityTablesTestContext db)
    {
        var authProvider = new AuthenticationProvider()
        {
            AuthenticationProviderName = "Local",
            AuthenticationProviderType = "Local"
        };
        db.AuthenticationProviders.Add(authProvider);
        db.SaveChanges();
        return authProvider;
    }
    private static void AddAdminRoleToAdminUser(SecurityTablesTestContext db, ApplicationRole adminRole, User adminUser)
    {
        var adminUserRole = new UserRole() { Role = adminRole, User = adminUser };
        db.UserRoles.Add(adminUserRole);
        db.SaveChanges();
    }
    private static void AddAuthenticationProviderMappingToAdminUser(SecurityTablesTestContext db, AuthenticationProvider localAuthProvider, User adminUser)
    {
        var adminAuthenticationProviderMapping = new UserAuthenticationProviderMapping()
        {
            AuthenticationProvider = localAuthProvider,
            User = adminUser,
            Username =
            adminUser.Username
        };
        db.UserAuthenticationProviderMappings.Add(adminAuthenticationProviderMapping);
        db.SaveChanges();
    }
    private static User CreateAdminUser(SecurityTablesTestContext db)
    {
        var adminUser = new User()
        {
            Username = "admin",
            Name = "admin",
            Email = "admin@admin.com",
            EmailConfirmed = true
        };

        db.Users.Add(adminUser);
        db.SaveChanges();
        return adminUser;
    }
    private static void AddAdminPermissionToAdminRole(SecurityTablesTestContext db, ApplicationPermission adminPermission, ApplicationRole adminRole)
    {
        var adminRolePermission = new ApplicationRolePermission()
        {
            RoleId = adminRole.Id,
            PermissionId = adminPermission.Id
        };
        db.ApplicationRolePermissions.Add(adminRolePermission);
        db.SaveChanges();
    }
    private static ApplicationRole CreateAdminRole(SecurityTablesTestContext db)
    {
        var adminRole = new ApplicationRole()
        {
            RoleName = "Admin",
            RoleDescription = "Admin Role"
        };
        db.ApplicationRoles.Add(adminRole);
        db.SaveChanges();
        return adminRole;
    }
    private static ApplicationRole CreateUserRole(SecurityTablesTestContext db)
    {
        var role = new ApplicationRole()
        {
            RoleName = "User",
            RoleDescription = "User Role"
        };
        db.ApplicationRoles.Add(role);
        db.SaveChanges();
        return role;
    }
    private static ApplicationPermission CreateAdminPermission(SecurityTablesTestContext db)
    {
        var adminPermission = new ApplicationPermission()
        {
            PermissionName = "Admin",
            PermissionDescription = "Admin Permission"
        };
        db.ApplicationPermissions.Add(adminPermission);
        db.SaveChanges();
        return adminPermission;
    }
}