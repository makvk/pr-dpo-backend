using TaskManager.Endpoints;

namespace TaskManager.Extensions;

public static class EndpointsExtension
{
    public static WebApplication MapApplicationEndpoints(this WebApplication app)
    {
        app.MapJsonEndpoints();

        return app;
    }
}