using TaskManager.Middleware;

namespace TaskManager.Extensions;

public static class MiddlewareExtension
{
    public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}