using HelpDesk.Results.Endpoints;

namespace HelpDesk.Results.Extensions;

public static class EndpointsExtension
{
    public static WebApplication UseEndpoints(this WebApplication app)
    {
        app.MapErrorEndpoints();
        app.MapBuisnessEndpoints();
        return app;
    }
}