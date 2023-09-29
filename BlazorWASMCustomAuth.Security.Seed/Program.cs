﻿using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Infrastructure;
using Microsoft.EntityFrameworkCore;
using BlazorWASMCustomAuth.Security.Shared;

internal class Program
{
    private static void Main(string[] args)
    {
        var options = new DbContextOptions<SecurityTablesTestContext>();

        //var db = new SecurityTablesTestContext(new DbContextOptionsBuilder<SecurityTablesTestContext>().UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SecurityTablesTest;Trusted_Connection=True;Trust Server Certificate=true").Options);
        var db = new SecurityTablesTestContext(new DbContextOptionsBuilder<SecurityTablesTestContext>().UseSqlServer("Server=.;Database=SecurityTablesTest;Trusted_Connection=True;Trust Server Certificate=true").Options);
        db.Database.Migrate(); //CREATES DATABASE IF IT DOESNT EXISTS
        db.Database.EnsureCreated(); //CREATES TABLES IF IT DOESNT EXISTS

        //Security App
        var newApp = CreateApplication(db);
                
        var adminPermission = CreateAppPermission(db, newApp.Id, "Admin", "Admin Permission");
        var permissionList = CreateApplicationPermissions(db, newApp); //Create list of permissions based on Security.Shared Permissions

        var adminRole = CreateApplicationRole(db, newApp.Id, "Admin", "Admin Role");
        
        var applicationAuthenticationProvider = AddLocalAuthenticationProviderToApplication(db, newApp, adminRole, "67A09D32-7664-49F8-8E8E-471A56A2858E");

        AddPermissionToRole(db, adminPermission.Id, adminRole.Id);
        var adminUser = CreateUser(db, "admin@admin.com", "Admin", "admin123");
        AddUserToApplicationUser(db, adminUser, newApp);
        AddAuthenticationProviderMappingToUser(db, applicationAuthenticationProvider, adminUser);
        AddRoleToUser(db, adminRole, adminUser);

        var userRole = CreateApplicationRole(db, newApp.Id, "User", "User Role");
        AddPermissionToRole(db, GetPermissionIdByName(Access.MyProfile.View, permissionList), userRole.Id);
        AddPermissionToRole(db, GetPermissionIdByName(Access.MyProfile.Edit, permissionList), userRole.Id);
        var regularUser = CreateUser(db, "user@user.com", "User", "user123");
        AddUserToApplicationUser(db, regularUser, newApp);
        AddAuthenticationProviderMappingToUser(db, applicationAuthenticationProvider, regularUser);
        AddRoleToUser(db, userRole, regularUser);

        //Test App
        var TestApp = CreateTestApp(db);
        
        var TestAppUserRole = CreateApplicationRole(db, TestApp.Id, "User", "UserRole");

        var testAppAuthenticationProvider = AddLocalAuthenticationProviderToApplication(db, TestApp, TestAppUserRole, "9EBA0CCD-FF5B-42AB-B6FB-861D18BD68D3");

        var counterPermission = CreateAppPermission(db, newApp.Id, "Counter", "Counter Permission");
        AddPermissionToRole(db, counterPermission.Id, TestAppUserRole.Id);
        
        var fetchDataPermission = CreateAppPermission(db, newApp.Id, "FetchData", "Fetch Data Permission");
        AddPermissionToRole(db, fetchDataPermission.Id, TestAppUserRole.Id);
        
        AddUserToApplicationUser(db, adminUser, TestApp);        
        AddAuthenticationProviderMappingToUser(db, testAppAuthenticationProvider, adminUser);
        AddRoleToUser(db, TestAppUserRole, adminUser);

    }

    private static int GetPermissionIdByName(string permissionName, Dictionary<string, ApplicationPermission> permissionList)
    {
        permissionList.TryGetValue(permissionName, out var value);
        return value.Id;
    }
    private static Dictionary<string, ApplicationPermission> CreateApplicationPermissions(SecurityTablesTestContext db, Application application)
    {
        var permissionList = Access.GetRegisteredPermissions();
        Dictionary<string, ApplicationPermission> outputList = new Dictionary<string, ApplicationPermission>();

        foreach (var permission in permissionList)
        {
            var newPermission = new ApplicationPermission() { ApplicationId = application.Id, PermissionName = permission, PermissionDescription = "" };
            outputList.Add(permission, newPermission);
            db.ApplicationPermissions.Add(newPermission);
        }
        db.SaveChanges();

        return outputList;
    }
    private static Application CreateApplication(SecurityTablesTestContext db)
    {
        Application newApplication = new Application()
        {
            ApplicationName = "DsK.AuthorizarionServer",
            ApplicationDesc = "Manages authentication and authorization for other applications",
            ApplicationGuid = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            AppApiKey = Guid.Parse("CAB41EEC-6002-4738-BE23-128B0A7276C1"),
            CallbackUrl = "/Callback/"
        };

        db.Applications.Add(newApplication);
        db.SaveChanges();
        return newApplication;
    }
    private static ApplicationAuthenticationProvider AddLocalAuthenticationProviderToApplication(SecurityTablesTestContext db, Application newApplication, ApplicationRole role,  string guid)
    {
        ApplicationAuthenticationProvider applicationAuthenticationProvider =
                new ApplicationAuthenticationProvider()
                {
                    ApplicationAuthenticationProviderGuid = Guid.Parse(guid),
                    ApplicationId = newApplication.Id,
                    DefaultApplicationRoleId = role.Id,
                    Name = "Local",
                    AuthenticationProviderType = "Local",
                    Domain = "",
                    Username = "",
                    Password = "",
                    ApplicationAuthenticationProviderDisabled = false,
                    RegistrationEnabled = true,
                    RegistrationAutoEmailConfirm = true                    
                };

        db.ApplicationAuthenticationProviders.Add(applicationAuthenticationProvider);
        db.SaveChanges();

        return applicationAuthenticationProvider;
    }
    private static void AddRoleToUser(SecurityTablesTestContext db, ApplicationRole adminRole, User adminUser)
    {
        var adminUserRole = new UserRole() { Role = adminRole, User = adminUser };
        db.UserRoles.Add(adminUserRole);
        db.SaveChanges();
    }
    private static void AddAuthenticationProviderMappingToUser(SecurityTablesTestContext db, ApplicationAuthenticationProvider authProvider, User adminUser)
    {
        var applicationAuthenticationProviderUserMapping = new ApplicationAuthenticationProviderUserMapping()
        {
            User = adminUser,
            Username = adminUser.Email,
            ApplicationAuthenticationProvider = authProvider
        };
        db.ApplicationAuthenticationProviderUserMappings.Add(applicationAuthenticationProviderUserMapping);
        db.SaveChanges();
    }
    private static User CreateUser(SecurityTablesTestContext db, string email, string name, string password)
    {
        var ramdomSalt = SecurityHelpers.RandomizeSalt;

        var adminUser = new User()
        {
            Email = email,
            Name = name,
            EmailConfirmed = true,
            AccessFailedCount = 0,
            LockoutEnabled = false,
            HashedPassword = SecurityHelpers.HashPasword(password, ramdomSalt),
            Salt = Convert.ToHexString(ramdomSalt),
            AccountCreatedDateTime = DateTime.Now,
            LastPasswordChangeDateTime = DateTime.Now

        };

        db.Users.Add(adminUser);
        db.SaveChanges();
        return adminUser;
    }
    private static ApplicationUser AddUserToApplicationUser(SecurityTablesTestContext db, User user, Application application)
    {
        var adminApplicationUser = new ApplicationUser()
        {
            UserId = user.Id,
            ApplicationId = application.Id,
            AccessFailedCount = 0,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        };

        db.ApplicationUsers.Add(adminApplicationUser);
        db.SaveChanges();
        return adminApplicationUser;
    }
    private static void AddPermissionToRole(SecurityTablesTestContext db, int permissionId, int roleId)
    {
        var rolePermission = new ApplicationRolePermission()
        {
            ApplicationRoleId = roleId,
            ApplicationPermissionId = permissionId
        };
        db.ApplicationRolePermissions.Add(rolePermission);
        db.SaveChanges();
    }
    private static ApplicationRole CreateApplicationRole(SecurityTablesTestContext db, int applicationId, string RoleName, string RoleDescription)
    {
        var adminRole = new ApplicationRole()
        {
            ApplicationId = applicationId,
            RoleName = RoleName,
            RoleDescription = RoleDescription
        };
        db.ApplicationRoles.Add(adminRole);
        db.SaveChanges();
        return adminRole;
    }
    private static Application CreateTestApp(SecurityTablesTestContext db)
    {
        Application newApplication = new Application()
        {
            ApplicationName = "TestApp.Client",
            ApplicationDesc = "Application to test DsK.AuthorizationServer",
            ApplicationGuid = Guid.Parse("004998CC-6A12-46AD-A7D3-D032B4194358"),
            AppApiKey = Guid.Parse("B4F1712E-25E9-4E35-A54A-68A8BA6CEB2C"),
            CallbackUrl = "https://localhost:7298/Callback/"
        };

        db.Applications.Add(newApplication);
        db.SaveChanges();
        return newApplication;
    }
    private static ApplicationPermission CreateAppPermission(SecurityTablesTestContext db, int applicationId, string permissionName, string permissionDescription)
    {
        var permission = new ApplicationPermission()
        {
            ApplicationId = applicationId,
            PermissionName = permissionName,
            PermissionDescription = permissionDescription
        };
        db.ApplicationPermissions.Add(permission);
        db.SaveChanges();

        return permission;
    }

}