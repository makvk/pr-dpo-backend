using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text;
using Microsoft.VisualBasic;
using StudentPortal.Diagnostics.Middleware;
using StudentPortal.Diagnostics.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddStudentPortalServices();

var app = builder.Build();
// app.Environment.EnvironmentName = "Production";

// error middleware задание 7
app.UseMiddleware<ErrorHandlingMiddleware>();

// 2 задание
app.Use(async (context, next) =>
{
    var timeService = app.Services.GetService<IDateTimeService>();
    var startTime = timeService?.GetTime();
    await next(context);
    var endTime = timeService?.GetTime();
    Console.WriteLine($"Start time: {startTime}, end time: {endTime}");
});

app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    
    var html = @"
    <!DOCTYPE html>
    <html>
        <head>
            <meta charset='utf-8'>
            <title>StudentPortal - Тестовое приложение</title>
        </head>
        <body>
            <h1>StudentPortal Тестовое приложение</h1>
            <p>Краткое описание проекта: приложение для проверки работы middleware, сервисов и маршрутизации</p>
            
            <h2>Тестовые маршруты</h2>
            <table border='1'>
                <tr>
                    <th>Запрос</th>
                    <th>Ожидаемое поведение</th>
                    <th>Какая тема проверяется</th>
                </tr>
                <tr>
                    <td><a href='/'>/</a></td>
                    <td>Стартовая страница приложения. Краткое описание проекта и ссылки на тестовые маршруты.</td>
                    <td>Терминальный обработчик</td>
                </tr>
                <tr>
                    <td><a href='/tools/time'>/tools/time</a></td>
                    <td>Возвращает текущее время через сервис.</td>
                    <td>Map + DI</td>
                </tr>
                <tr>
                    <td><a href='/tools/date'>/tools/date</a></td>
                    <td>Возвращает текущую дату через сервис.</td>
                    <td>Map + DI</td>
                </tr>
                <tr>
                    <td><a href='/tools/info'>/tools/info</a></td>
                    <td>Отдает краткую диагностическую информацию о приложении.</td>
                    <td>Вложенные ветки Map</td>
                </tr>
                <tr>
                    <td><a href='/tools/time?trace=true'>/tools/time?trace=true</a></td>
                    <td>Сначала выполняется ветка UseWhen, затем основной обработчик /tools/time.</td>
                    <td>UseWhen</td>
                </tr>
                <tr>
                    <td><a href='/anything?format=plain'>/anything?format=plain</a></td>
                    <td>Запрос должен быть перехвачен отдельной веткой и вернуть plain-text ответ.</td>
                    <td>MapWhen</td>
                </tr>
                <tr>
                    <td><a href='/secure/report'>/secure/report</a></td>
                    <td>Без токена должен вернуть 403 и текст ошибки.</td>
                    <td>Класс middleware</td>
                </tr>
                <tr>
                    <td><a href='/secure/report?token=study2026'>/secure/report?token=study2026</a></td>
                    <td>При валидном токене доступ разрешен.</td>
                    <td>Класс middleware + extension</td>
                </tr>
                <tr>
                    <td><a href='/env'>/env</a></td>
                    <td>Показывает сведения об окружении и путях приложения.</td>
                    <td>IWebHostEnvironment</td>
                </tr>
                <tr>
                    <td><a href='/di/services'>/di/services</a></td>
                    <td>Выводит сведения о сервисах из IServiceCollection.</td>
                    <td>IServiceCollection</td>
                </tr>
                <tr>
                    <td><a href='/unknown'>/unknown</a></td>
                    <td>Возвращает 404 и сообщение Not Found.</td>
                    <td>Порядок pipeline + ErrorHandlingMiddleware</td>
                </tr>
                <tr>
                    <td><a href='/genpass'>/genpass</a></td>
                    <td>Генерирует пароль с помощью встроенного сервиса.</td>
                    <td>Создание собственных сервисов</td>
                </tr>
            </table>
        </body>
    </html>";
    
    await context.Response.WriteAsync(html);
});

// дополнительное логирование 3
app.UseWhen(context => context.Request.Query["trace"] == "true", async appBuilder => 
{
    appBuilder.Use(async (context, next) => 
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Начало обработки запроса");
        await next(context);
        logger.LogInformation("Конец обработки запроса");
    });
});

// 4 задание ветка MapWhen
app.MapWhen(context => context.Request.Query["format"] == "plain", async appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync("Самостоятельный ответ ветки MapWhen");
    });
});

// ветка tools 5
app.Map("/tools", async appBuilder => 
{
    appBuilder.Map("/time", WriteTime);
    appBuilder.Map("/date", WriteDate);
    appBuilder.Map("/info", WriteInfo);
});

// ветка secure 6
app.Map("/secure", appBuilder =>
{
    appBuilder.UseToken("study2026");
    appBuilder.Map("/report", appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.WriteAsync("Доступ разрешен");
        });
    });
});

// endpoint env 8
app.MapGet("/env", async (HttpContext context, IWebHostEnvironment env) =>
{
    var sb = new StringBuilder();
    sb.Append("Env name: ");
    sb.Append(env.EnvironmentName);
    sb.Append("\nApp name: ");
    sb.Append(env.ApplicationName);
    sb.Append("\nContent root path: ");
    sb.Append(env.ContentRootPath);
    sb.Append("\nWeb root path: ");
    sb.Append(env.WebRootPath);
    var msg = sb.ToString();

    if (env.IsProduction()) {
        await context.Response.WriteAsync("\t\tProd!!!\n\n\n");
    } else if (env.IsDevelopment()) {
        await context.Response.WriteAsync("\t\tDev!!!\n\n\n");
    }
    await context.Response.WriteAsync(msg);
});

// endpoint /di/services 9 
app.MapGet("/di/services", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    var msg = BuildTable(services);
    await context.Response.WriteAsync(msg);
});

app.MapGet("/genpass", async (HttpContext context) =>
{
    var genService = app.Services.GetService<IGenerateService>();
    await context.Response.WriteAsync("Password: " + genService?.GenPassword());
});

app.Run();

void WriteTime(IApplicationBuilder appBuilder)
{
    var timeService = app.Services.GetService<IDateTimeService>();
    appBuilder.Run(async context => await context.Response.WriteAsync($"Current time: {timeService?.GetTime()}"));
}

void WriteDate(IApplicationBuilder appBuilder)
{
    var timeService = app.Services.GetService<IDateTimeService>();
    appBuilder.Run(async context => await context.Response.WriteAsync($"Current date: {timeService?.GetDate()}"));
}

void WriteInfo(IApplicationBuilder appBuilder)
{
    appBuilder.Run(async context => await context.Response.WriteAsync("Information"));
}

string BuildTable(IServiceCollection services)
{
    var sb = new StringBuilder();
    sb.Append($"<h1>All services: {services.Count()}</h1>");
    sb.Append("<table>");
    sb.Append("<tr><th>Тип</th><th>Lifetime</th><th>Реализация</th></tr>");

    foreach (var svc in services)
    {
        sb.Append("<tr>");
        sb.Append($"<td>{svc.ServiceType.FullName}</td>");
        sb.Append($"<td>{svc.Lifetime}</td>");
        sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
        sb.Append("</tr>");
    }

    sb.Append("</table>");

    return sb.ToString();
}