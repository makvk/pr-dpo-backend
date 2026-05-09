using TaskManager.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine("WebApplication Создан");
Console.WriteLine($"Окружение {builder.Environment.EnvironmentName}");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseLoggingMiddleware();

app.MapApplicationEndpoints();

app.Run();
