using DsK.AuthServer.Security.EntityFramework.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DsK.AuthServer.Security.Infrastructure;

public class LoggingMiddleware
{
    private readonly RequestDelegate requestProcess;
    private readonly ILogger _logger;


    public LoggingMiddleware(RequestDelegate requestProcess, ILoggerFactory loggerFactory)
    {
        this.requestProcess = requestProcess;
        _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
    }

    public async Task Invoke(HttpContext context, DsKauthServerContext db)
    {
        await LogRequest(context, db);
        await requestProcess(context);
    }
    private async Task LogRequest(HttpContext context, DsKauthServerContext db)
    {
        //todo : implement applicationid in log
        var userid = context.User.Claims.Where(_ => _.Type == "UserId").Select(_ => _.Value).FirstOrDefault();
        int userIdParsed = 0;
        if (userid != null)
            userIdParsed = int.Parse(userid);
        UserLog userLog = new UserLog()
        {
            UserId = userIdParsed,
            ApplicationId = 1,
            Method = context.Request.Method,
            Ip = context.Connection.RemoteIpAddress.ToString(),
            Path = context.Request.Path,
            QueryString = context.Request.QueryString.ToString(),
            LogDateTime = DateTime.Now
        };

        db.UserLogs.Add(userLog);
        db.SaveChanges();

        _logger.LogInformation($"Http Request Information:\n" +
        $"Schema:{context.Request.Scheme}\n " +
                               $"Path: {context.Request.Path}\n " +
                               $"QueryString: {context.Request.QueryString}\n ");
    }
}
public static class RequestResponseLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}
