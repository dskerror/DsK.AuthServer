using BlazorWASMCustomAuth.Security.EntityFramework.Models;
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

        Application newApplication = CreateApplication(db);        
        ApplicationAuthenticationProvider applicationAuthenticationProvider = AddLocalAuthenticationProviderToApplication(db, newApplication);
        ApplicationPermission adminPermission = CreateAdminPermission(db);
        ApplicationRole adminRole = CreateAdminRole(db);
        CreateUserRole(db);
        AddAdminPermissionToAdminRole(db, adminPermission, adminRole);
        User adminUser = CreateAdminUser(db);
        ApplicationUser adminApplicationUser = AddAdminUserToApplicationUser(db, adminUser);
        AddAuthenticationProviderMappingToAdminUser(db, applicationAuthenticationProvider, adminUser);
        AddRoleToUser(db, adminRole, adminUser);
        var permissionList = Access.GetRegisteredPermissions();
        foreach (var permission in permissionList)
        {
            db.ApplicationPermissions.Add(new ApplicationPermission() { ApplicationId = 1, PermissionName = permission, PermissionDescription = "" });
        }
        db.SaveChanges();



        Application TestApp = CreateTestApp(db);
        ApplicationRole TestAppUserRole = CreateTestAppUserRole(db);
        CreateTestAppPermissions(db);
        AddAdminPermissionToAdminRole(db, TestAppUserRole);
        AddAdminUserToTestApp(db, adminUser);        
        ApplicationAuthenticationProvider testAppAuthenticationProvider = AddLocalAuthenticationProviderToApplication(db, TestApp);
        AddAuthenticationProviderMappingToAdminUser(db, testAppAuthenticationProvider, adminUser);
        AddRoleToUser(db, TestAppUserRole, adminUser);

    }

    private static ApplicationAuthenticationProvider AddLocalAuthenticationProviderToApplication(SecurityTablesTestContext db, Application newApplication)
    {
        ApplicationAuthenticationProvider applicationAuthenticationProvider =
                new ApplicationAuthenticationProvider()
                {
                    ApplicationId = newApplication.Id,
                    Name = "Local",
                    AuthenticationProviderType = "Local",
                    Domain = "",
                    Username = "",
                    Password = "",
                    ApplicationAuthenticationProviderDisabled = false,
                    RegistrationEnabled = true,
                };

        db.ApplicationAuthenticationProviders.Add(applicationAuthenticationProvider);
        db.SaveChanges();

        return applicationAuthenticationProvider;
    }
    private static Application CreateApplication(SecurityTablesTestContext db)
    {
        Application newApplication = new Application()
        {
            ApplicationName = "DsK.AuthorizarionServer",
            ApplicationDesc = "Manages authentication and authorization for other applications",
            ApplicationGuid = Guid.Parse("D9847B27-B28A-4223-B7F7-ACD0C365331C"),
            AppApiKey = Guid.Parse("CAB41EEC-6002-4738-BE23-128B0A7276C1"),
            CallbackUrl = "/Callback/"
        };

        db.Applications.Add(newApplication);
        db.SaveChanges();
        return newApplication;
    }
    private static void AddRoleToUser(SecurityTablesTestContext db, ApplicationRole adminRole, User adminUser)
    {
        var adminUserRole = new UserRole() { Role = adminRole, User = adminUser };
        db.UserRoles.Add(adminUserRole);
        db.SaveChanges();
    }
    private static void AddAuthenticationProviderMappingToAdminUser(SecurityTablesTestContext db, ApplicationAuthenticationProvider authProvider, User adminUser)
    {
        var adminAuthenticationProviderMapping = new UserAuthenticationProviderMapping()
        {   
            User = adminUser,
            Username = adminUser.Email,
            ApplicationAuthenticationProvider = authProvider
        };
        db.UserAuthenticationProviderMappings.Add(adminAuthenticationProviderMapping);
        db.SaveChanges();
    }
    private static User CreateAdminUser(SecurityTablesTestContext db)
    {
        var ramdomSalt = SecurityHelpers.RandomizeSalt;

        var adminUser = new User()
        {
            Email = "admin@admin.com",
            Name = "admin",            
            EmailConfirmed = true,
            AccessFailedCount = 0,            
            LockoutEnabled = false,
            HashedPassword = SecurityHelpers.HashPasword("admin123", ramdomSalt),
            Salt = Convert.ToHexString(ramdomSalt),
            AccountCreatedDateTime = DateTime.Now,
            LastPasswordChangeDateTime = DateTime.Now
            
        };

        db.Users.Add(adminUser);
        db.SaveChanges();
        return adminUser;
    }
    private static ApplicationUser AddAdminUserToApplicationUser(SecurityTablesTestContext db, User user)
    {
        var adminApplicationUser = new ApplicationUser()
        {
            UserId = user.Id,
            ApplicationId = 1,
            AccessFailedCount = 0,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        };

        db.ApplicationUsers.Add(adminApplicationUser);
        db.SaveChanges();
        return adminApplicationUser;
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
            ApplicationId = 1,
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
            ApplicationId = 1,
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
            ApplicationId = 1,
            PermissionName = "Admin",
            PermissionDescription = "Admin Permission"
        };
        db.ApplicationPermissions.Add(adminPermission);
        db.SaveChanges();
        return adminPermission;
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
    private static ApplicationRole CreateTestAppUserRole(SecurityTablesTestContext db)
    {
        var role = new ApplicationRole()
        {
            ApplicationId = 2,
            RoleName = "User Role",
            RoleDescription = "User Role"
        };
        db.ApplicationRoles.Add(role);
        db.SaveChanges();
        return role;
    }
    private static void CreateTestAppPermissions(SecurityTablesTestContext db)
    {
        var counterPermission = new ApplicationPermission()
        {
            ApplicationId = 2,
            PermissionName = "Counter",
            PermissionDescription = "Counter Permission"
        };
        db.ApplicationPermissions.Add(counterPermission);

        var fetchDataPermission = new ApplicationPermission()
        {
            ApplicationId = 2,
            PermissionName = "FetchData",
            PermissionDescription = "Fetch Data Permission"
        };
        db.ApplicationPermissions.Add(fetchDataPermission);

        db.SaveChanges();
    }
    private static void AddAdminPermissionToAdminRole(SecurityTablesTestContext db, ApplicationRole TestAppUserRole)
    {
        var counterRolePermission = new ApplicationRolePermission()
        {
            RoleId = TestAppUserRole.Id,
            PermissionId = 38
        };
        db.ApplicationRolePermissions.Add(counterRolePermission);

        var fetchdataRolePermission = new ApplicationRolePermission()
        {
            RoleId = TestAppUserRole.Id,
            PermissionId = 39
        };
        db.ApplicationRolePermissions.Add(fetchdataRolePermission);
        db.SaveChanges();
    }

    private static ApplicationUser AddAdminUserToTestApp(SecurityTablesTestContext db, User user)
    {
        var adminApplicationUser = new ApplicationUser()
        {
            UserId = user.Id,
            ApplicationId = 2,
            AccessFailedCount = 0,
            TwoFactorEnabled = false,
            LockoutEnabled = false
        };

        db.ApplicationUsers.Add(adminApplicationUser);
        db.SaveChanges();
        return adminApplicationUser;
    }
}