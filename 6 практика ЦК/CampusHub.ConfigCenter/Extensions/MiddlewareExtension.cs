using CampusHub.ConfigCenter.Middleware;

namespace CampusHub.ConfigCenter.Extensions;

public static class MiddlewareExtension
{
    public static WebApplication UsePortalMiddleware(this WebApplication app)
    {
        app.UseMiddleware<PortalHeaderMiddleware>();
        
        return app;
    }
}