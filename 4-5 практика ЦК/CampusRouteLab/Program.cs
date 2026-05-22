using System.Runtime.Serialization;
using CampusRouteLab.Middleware;
using CampusRouteLab.Services;
using CampusRouteLab.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddCampusServices();

var app = builder.Build();

app.UseStaticFiles();

app.UseRequestMiddleware();
app.UseEndpoints();

app.Run();
