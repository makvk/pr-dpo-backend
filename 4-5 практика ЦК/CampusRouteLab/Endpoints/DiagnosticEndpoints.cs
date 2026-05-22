using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using CampusRouteLab.Services;
using CampusRouteLab.Models;

namespace CampusRouteLab.Endpoints;

public static class DiagnosticEndpoints
{
    public static WebApplication MapDiagnosticEndpoints(this WebApplication app)
    {
        app.MapGet("/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
        {
            var msg = string.Join("\n", endpointSources.SelectMany(source => source.Endpoints));

            return Results.Text(msg);
        });

        app.MapGet("/diag/lifetimes", (DiagnosticsReportService service) =>
        {
            List<Lifetime> res = new()
            {
                new Lifetime {Name = "appInfo", Id = service.appInfo.AppInstanceId, Time = service.appInfo.StartedAt},
                new Lifetime {Name = "requestContext", Id = service.requestContext.RequestId, Time = service.requestContext.CreatedAt},
                new Lifetime {Name = "transientMarker", Id = service.transientMarker.MarkerId, Time = service.transientMarker.MarkerTime},
            };
            return Results.Json(res);
        });

        app.MapGet("/diag/lifetimes/check", (
                DiagnosticsReportService service,
                IAppInfoService appInfo,
                IRequestContextService requestContext,
                ITransientMarkerService transientMarker
            ) =>
        {
            List<Lifetime> res = new()
            {
                new Lifetime {Name = "appInfo", Id = service.appInfo.AppInstanceId, Time = service.appInfo.StartedAt},
                new Lifetime {Name = "requestContext", Id = service.requestContext.RequestId, Time = service.requestContext.CreatedAt},
                new Lifetime {Name = "transientMarker", Id = service.transientMarker.MarkerId, Time = service.transientMarker.MarkerTime},

                new Lifetime {Name = "appInfo new", Id = appInfo.AppInstanceId, Time = appInfo.StartedAt},
                new Lifetime {Name = "requestContext new", Id = requestContext.RequestId, Time = requestContext.CreatedAt},
                new Lifetime {Name = "transientMarker new", Id = transientMarker.MarkerId, Time = transientMarker.MarkerTime},
            };
            return Results.Json(res);
        });

        app.MapGet("/diag/request-services", (HttpContext context) =>
        {
            IRequestContextService service = context.RequestServices.GetRequiredService<IRequestContextService>();

            Lifetime res = new()
            {
                Name = "requestContext", Id = service.RequestId, Time = service.CreatedAt
            };

            return Results.Json(res);
        });

        app.MapGet("/diag/app-services", GetAppServices);
        return app;
    }

    private static IResult GetAppServices(HttpContext context)
    {
        IAppInfoService service = context.RequestServices.GetRequiredService<IAppInfoService>();

        Lifetime res = new()
        {
            Name = "appInfo", 
            Id = service.AppInstanceId, 
            Time = service.StartedAt
        };

        return Results.Json(res);
    }
}