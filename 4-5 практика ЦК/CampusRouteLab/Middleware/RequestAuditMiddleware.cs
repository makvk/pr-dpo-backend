using System.ComponentModel;
using System.Net;
using Microsoft.VisualBasic;
using CampusRouteLab.Services;

namespace CampusRouteLab.Middleware;

public class RequestAuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAppInfoService _appInfo;
    private readonly ILogger<RequestAuditMiddleware> _logger;
    public RequestAuditMiddleware(RequestDelegate next, IAppInfoService appInfo, ILogger<RequestAuditMiddleware> logger)
    {
        _next = next;
        _appInfo = appInfo;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context, 
        IRequestContextService requestContext, 
        ITransientMarkerService transientMarker
    )
    {
        _logger.LogInformation("Начало обработки запроса");
        
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append("X-App-Instance", _appInfo.AppInstanceId.ToString());
            context.Response.Headers.Append("X-Request-Id", requestContext.RequestId.ToString());
            context.Response.Headers.Append("X-Transient-Id", transientMarker.MarkerId.ToString());

            return Task.CompletedTask;
        });

        await _next(context);
        
        if (context.Request.Path.StartsWithSegments("/diag"))
        {
            _logger.LogInformation("Заголовки: singleton, scooped, transient");
            _logger.LogInformation($"{context.Response.Headers["X-App-Instance"]}, {context.Response.Headers["X-Request-Id"]}, {context.Response.Headers["X-Transient-Id"]}");
        }
        _logger.LogInformation("Конец обработки запроса");
    }
}