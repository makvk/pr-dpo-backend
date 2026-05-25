using System.Diagnostics;
using CampusHub.ConfigCenter.Models;
using Microsoft.Extensions.Options;

namespace CampusHub.ConfigCenter.Middleware;

public class PortalHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptions<PortalOptions> _options;
    public PortalHeaderMiddleware(RequestDelegate next, IOptions<PortalOptions> options)
    {
        _next = next;
        _options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Append("X-Portal-Title", _options.Value.Title);
        context.Response.Headers.Append("X-Portal-Semester", _options.Value.Semester);
        await _next(context);
    }
}