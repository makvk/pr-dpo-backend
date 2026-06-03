using HelpDesk.Results.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddTicketServices();

var app = builder.Build();

// app.Environment.EnvironmentName = "Production";

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(app => app.Run(async context =>
    {
        await context.Response.WriteAsync($"Error: 500");
    }));
}
app.UseStatusCodePagesWithRedirects("/error/status/{0}");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseEndpoints();

app.Run();
