using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

internal class Program
{
    private static void Main(string[] args)
    {
        var options = new DbContextOptions<SecurityTablesTestContext>();
        
        var db = new SecurityTablesTestContext(new DbContextOptionsBuilder<SecurityTablesTestContext>().UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SecurityTablesTest;Trusted_Connection=True;Trust Server Certificate=true").Options);
        //Server=.;Database=SecurityTablesTest;Trusted_Connection=True;Trust Server Certificate=true");
        //Data Source=(localdb)\MSSQLLocalDB;
        db.Database.Migrate(); //CREATES DATABASE IF IT DOESNT EXISTS

        Permission adminPermission = CreateAdminPermission(db);
        Role adminRole = CreateAdminRole(db);
        AddAdminPermissionToAdminRole(db, adminPermission, adminRole);
        User adminUser = CreateAdminUser(db);
        AddAdminRoleToAdminUser(db, adminRole, adminUser);
        AuthenticationProvider authProvider = CreateLocalAuthProvider(db);
        CreateAdminUserPassword(db, adminUser);

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
            AuthenticationProviderName = "Local"
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