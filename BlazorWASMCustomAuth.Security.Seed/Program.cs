using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Infrastructure;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var db = new SecurityTablesTestContext();
        db.Database.Migrate(); //CREATES DATABASE IF IT DOESNT EXISTS

        var adminPermission = new Permission()
        {
            PermissionName = "Admin",
            PermissionDescription = "Admin Permission"
        };
        db.Permissions.Add(adminPermission);

        var adminRole = new Role()
        {
            RoleName = "Admin",
            RoleDescription = "Admin Role"
        };
        db.Roles.Add(adminRole);
        db.SaveChanges();

        var adminRolePermission = new RolePermission()
        {
            RoleId = adminRole.Id,
            PermissionId = adminPermission.Id
        };
        db.RolePermissions.Add(adminRolePermission);
        db.SaveChanges();


        var adminUser = new User()
        {
            Username = "admin",
            Name = "admin",
            Email = "admin@admin.com",
            EmailConfirmed = true
        };

        db.Users.Add(adminUser);
        db.SaveChanges();

        adminUser.Roles.Add(adminRole);
        db.SaveChanges();

        var authProvider = new AuthenticationProvider()
        {
            AuthenticationProviderName = "Local"
        };
        db.AuthenticationProviders.Add(authProvider);
        db.SaveChanges();

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

        var userAuthenticationProvider = new UserAuthenticationProvider
        {
            UserId = adminUser.Id,
            AuthenticationProviderId = authProvider.Id,
            MappedUsername = adminUser.Username
        };
        db.UserAuthenticationProviders.Add(userAuthenticationProvider);
        db.SaveChanges();

        var userAuthenticationProviderDefault = new UserAuthenticationProviderDefault
        {
            UserId = adminUser.Id,
            AuthenticationProviderId = authProvider.Id,
        };
        db.UserAuthenticationProviderDefaults.Add(userAuthenticationProviderDefault);
        db.SaveChanges();


        IList<Permission> permissions = new List<Permission>() {
            new Permission() { PermissionName = "PermissionsGet", PermissionDescription = "" },
            new Permission() { PermissionName = "RefreshToken", PermissionDescription = "" },
            new Permission() { PermissionName = "RolePermissionsGet", PermissionDescription = "" },
            new Permission() { PermissionName = "RolesGet", PermissionDescription = "" },
            new Permission() { PermissionName = "UserChangeLocalPassword", PermissionDescription = "" },
            new Permission() { PermissionName = "UserCreate", PermissionDescription = "" },
            new Permission() { PermissionName = "UserCreateLocalPassword", PermissionDescription = "" },
            new Permission() { PermissionName = "UserLogin", PermissionDescription = "" },
            new Permission() { PermissionName = "UsersGet", PermissionDescription = "" }
         };
        db.Permissions.AddRange(permissions);
        db.SaveChanges();

    }
}