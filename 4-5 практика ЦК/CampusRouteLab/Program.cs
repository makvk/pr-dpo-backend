using System.Runtime.Serialization;
using CampusRouteLab.Middleware;
using CampusRouteLab.Services;
using CampusRouteLab.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.AddCampusServices();

var app = builder.Build();

app.UseRequestMiddleware();
app.UseEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
