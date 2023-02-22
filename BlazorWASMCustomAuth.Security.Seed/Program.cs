using BlazorWASMCustomAuth.Security.EntityFramework.Models;
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


        AuthenticationProvider localAuthProvider = CreateLocalAuthProvider(db);
        Permission adminPermission = CreateAdminPermission(db);
        Role adminRole = CreateAdminRole(db);
        CreateUserRole(db);
        AddAdminPermissionToAdminRole(db, adminPermission, adminRole);
        User adminUser = CreateAdminUser(db);
        AddAuthenticationProviderToAdminUser(db, localAuthProvider, adminUser);
        AddAdminRoleToAdminUser(db, adminRole, adminUser);
        
        CreateAdminUserPassword(db, adminUser);

        var permissionList = BlazorWASMCustomAuth.Security.Shared.Constants.Access.GetRegisteredPermissions();
        foreach (var permission in permissionList)
        {
            db.Permissions.Add(new Permission() { PermissionName = permission, PermissionDescription = "" });
        }
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
    private static AuthenticationProvider CreateLocalAuthProvider(SecurityTablesTestContext db)
    {
        var authProvider = new AuthenticationProvider()
        {
            AuthenticationProviderName = "Local",
            AuthenticationProviderType= "Local",
            Domain="",
            Username="",
            Password=""
        };
        db.AuthenticationProviders.Add(authProvider);
        db.SaveChanges();
        return authProvider;
    }
    private static void AddAdminRoleToAdminUser(SecurityTablesTestContext db, Role adminRole, User adminUser)
    {
        var adminUserRole = new UserRole() { Role = adminRole, User = adminUser };
        db.UserRoles.Add(adminUserRole);
        db.SaveChanges();
    }    
    private static void AddAuthenticationProviderToAdminUser(SecurityTablesTestContext db, AuthenticationProvider localAuthProvider, User adminUser)
    {
        var adminAuthenticationProvider = new UserAuthenticationProvider() { 
            AuthenticationProvider = localAuthProvider, 
            User = adminUser, Username = 
            adminUser.Username 
        };
        db.UserAuthenticationProviders.Add(adminAuthenticationProvider);
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
    private static void AddAdminPermissionToAdminRole(SecurityTablesTestContext db, Permission adminPermission, Role adminRole)
    {
        var adminRolePermission = new RolePermission()
        {
            RoleId = adminRole.Id,
            PermissionId = adminPermission.Id
        };
        db.RolePermissions.Add(adminRolePermission);
        db.SaveChanges();
    }
    private static Role CreateAdminRole(SecurityTablesTestContext db)
    {
        var adminRole = new Role()
        {
            RoleName = "Admin",
            RoleDescription = "Admin Role"
        };
        db.Roles.Add(adminRole);
        db.SaveChanges();
        return adminRole;
    }
    private static Role CreateUserRole(SecurityTablesTestContext db)
    {
        var role = new Role()
        {
            RoleName = "User",
            RoleDescription = "User Role"
        };
        db.Roles.Add(role);
        db.SaveChanges();
        return role;
    }
    private static Permission CreateAdminPermission(SecurityTablesTestContext db)
    {
        var adminPermission = new Permission()
        {
            PermissionName = "Admin",
            PermissionDescription = "Admin Permission"
        };
        db.Permissions.Add(adminPermission);
        db.SaveChanges();
        return adminPermission;
    }
}