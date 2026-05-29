using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using CampusHub.ConfigCenter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;

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
                Portal = portalOptions.Value, 
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
            string sectionName = context.Request.Query["section"].ToString();
            if (sectionName == "")
            {
                sectionName = "Portal";
            }
            Console.WriteLine(sectionName);
            var section = appConfig.GetSection(sectionName);
            var res = GetSectionContent(section);

            return Results.Text(res);
        });

        app.MapGet("/config/connection", (IConfiguration appConfig) =>
        {
            var con = appConfig.GetConnectionString("DefaultConnection");

            return Results.Text(con);
        });

        app.MapGet("/config/raw", (IConfiguration appConfig) =>
        {
            return Results.Json(
                new Dictionary<string, string?>
                {
                    {"Title", appConfig.GetSection("Portal:Title").Value},
                    {"SupportEmail", appConfig.GetSection("Portal:SupportEmail").Value},
                    {"Sender", appConfig.GetSection("Notifications:Sender").Value},
                }
            );
        });

        app.MapGet("/config/section/portal", (IConfiguration appConfig) =>
        {
            var section = appConfig.GetSection("Portal");

            return Results.Json(section);
        });

        app.MapGet("/config/custom", (IConfiguration appConfig) =>
        {
            return Results.Json(
                new Dictionary<string, string?>
                {
                    {"ApiKey", appConfig["CustomProvider:ApiKey"]},
                    {"Endpoint", appConfig["CustomProvider:Endpoint"]},
                }
            );
        });
        app.MapGet("/config/effective", (IConfiguration appConfig) =>
        {
            return Results.Json(
                new Dictionary<string, string?>
                {
                    {"Env", appConfig["ASPNETCORE_ENVIRONMENT"]},
                    {"Title", appConfig["Portal:Title"]},
                    {"SupportEmail", appConfig["Portal:SupportEmail"]},
                    {"Sender", appConfig["Notifications:Sender"]},
                }
            );
        });

        return app;
    }

    private static string GetSectionContent(IConfiguration configSection)
    {
        StringBuilder contentBuilder = new();
        foreach (var section in configSection.GetChildren())
        {
            contentBuilder.Append($"\"{section.Key}\":");
            if (section.Value == null)
            {
                string subSectionContent = GetSectionContent(section);
                contentBuilder.Append($"{{\n{subSectionContent}}},\n");
            }
            else
            {
                contentBuilder.Append($"\"{section.Value}\",\n");
            }
        }
        return contentBuilder.ToString();
    }
}