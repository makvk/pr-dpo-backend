using System.Runtime.CompilerServices;
using System.Text;
using CampusHub.ConfigCenter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace CampusHub.ConfigCenter.Endpoints;

public static class ConfigEndpoints
{
    public static WebApplication MapConfigEnpoints(this WebApplication app)
    {
        app.MapGet("/config/bind", (IConfiguration appConfig) =>
        {
            var options = appConfig.GetSection("Portal").Get<PortalOptions>() ?? new PortalOptions();
            
            return Results.Json(options);
        });

        app.MapGet("/config/options", (
            IOptions<PortalOptions> portalOptions, 
            IOptions<NotificationOptions> notificationOptions) =>
        {
            return Results.Json(new
            {
                Portal = notificationOptions.Value, 
                Notifications = notificationOptions.Value
            });
        });

        app.MapGet("/config/providers", (IConfiguration configuration) =>
        {
            if (configuration is IConfigurationRoot configurationRoot) {
                var providersInfo = new List<object>();
                var providers = configurationRoot.Providers;
                foreach (var provider in providers)
                {
                    var info = new Dictionary<string, string?> 
                    {
                        {"name", provider.GetType().FullName},
                    };
                    if (provider is FileConfigurationProvider fileProvider)
                    {
                        info["source"] = fileProvider.Source.Path;
                    }
                    providersInfo.Add(info);
                }
                return Results.Json(providersInfo);
            }
            return Results.Problem("Не удалось получить корень конфигурации");
        });

        app.MapGet("/config/tree", (HttpContext context, IConfiguration appConfig) =>
        {
            string? sectionName = context.Request.Query["section"].ToString();
            
            var section = appConfig.GetSection(sectionName);
            while (section.GetChildren() != null)
            {
                
            }
            return Results.Json(section);
        });
        return app;
    }
}