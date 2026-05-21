namespace CampusRouteLab.Endpoints;

public static class EndpointsExtension
{
    public static WebApplication UseEndpoints(this WebApplication app)
    {
        app.MapBuisnessEndpoints();
        app.MapDiagnosticEndpoints();
        return app;
    }
}