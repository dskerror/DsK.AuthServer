using BlazorWASMCustomAuth.Security.EntityFramework.Models;
using BlazorWASMCustomAuth.Security.Infrastructure;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var db = new SecurityTablesTestContext();
        db.Database.Migrate(); //CREATES DATABASE IF IT DOESNT EXISTS

        var adminRole = new Role()
        {
            RoleName = "admin",
            RoleDescription = "admin"
        };
        db.Roles.Add(adminRole);
        db.SaveChanges();


        var adminUser = new User()
        {
            Username = "admin",
            Name = "admin",
            Email = "admin@admin.com",
            EmailConfirmed = true
        };
        adminUser.Roles.Add(adminRole);

        db.Users.Add(adminUser);
        
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
            UserId = 1,
            HashedPassword = SecurityHelpers.HashPasword("admin123", ramdomSalt),
            Salt = Convert.ToHexString(ramdomSalt),
            DateCreated = DateTime.Now
        };
        db.UserPasswords.Add(userPassword);
        db.SaveChanges();
    
    }
}