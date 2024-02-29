using DsK.AuthServer.Security.EntityFramework.Models;
using DsK.AuthServer.Security.Infrastructure;
using Microsoft.EntityFrameworkCore;
using DsK.AuthServer.Security.Shared;

internal class Program
{
    private static void Main(string[] args)
    {
        var options = new DbContextOptions<DsKauthServerContext>();
        //var db = new SecurityTablesTestContext(new DbContextOptionsBuilder<SecurityTablesTestContext>().UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DsKAuthServer;Trusted_Connection=True;Trust Server Certificate=true").Options);
        var db = new DsKauthServerContext(new DbContextOptionsBuilder<DsKauthServerContext>().UseSqlServer("Server=.;Database=DsKAuthServer;Trusted_Connection=True;Trust Server Certificate=true").Options);
    }
}