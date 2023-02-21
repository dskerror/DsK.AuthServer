using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BlazorWASMCustomAuth.Security.Infrastructure;

public class LoggingMiddleware
{
    private readonly RequestDelegate requestProcess;
    private readonly ILogger _logger;


    public LoggingMiddleware(RequestDelegate requestProcess, ILoggerFactory loggerFactory)
    {
        this.requestProcess = requestProcess;
        _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
    }


    public async Task Invoke(HttpContext context)
    {
        await LogRequest(context);
        await requestProcess(context);
    }


    private async Task LogRequest(HttpContext context)
    {
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
