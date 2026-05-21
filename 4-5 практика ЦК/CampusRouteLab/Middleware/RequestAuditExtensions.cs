namespace CampusRouteLab.Middleware;

public static class RequestAuditExtentions
{
    public static IApplicationBuilder UseRequestMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestAuditMiddleware>();
        return app;
    }
}