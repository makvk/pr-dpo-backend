namespace CampusHub.ConfigCenter.Extensions;

using CampusHub.ConfigCenter.Endpoints;
public static class EndpointsExtension
{
    public static WebApplication UseEndpoints(this WebApplication app)
    {
        app.MapConfigEnpoints();
        return app;
    }
}