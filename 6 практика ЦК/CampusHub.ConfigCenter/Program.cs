using CampusHub.ConfigCenter.Configuration;
using CampusHub.ConfigCenter.Extensions;
using CampusHub.ConfigCenter.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddXmlFile("portal.xml")
    .AddIniFile("notifications.ini")
    .AddTextFile("customsettings.txt")
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        {"Notifications:Sender", "in-memory@sender"}
    });

builder.Services.Configure<PortalOptions>(builder.Configuration.GetSection("Portal"));
builder.Services.Configure<NotificationOptions>(builder.Configuration.GetSection("Notifications"));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UsePortalMiddleware();
app.UseEndpoints();

app.Run();
