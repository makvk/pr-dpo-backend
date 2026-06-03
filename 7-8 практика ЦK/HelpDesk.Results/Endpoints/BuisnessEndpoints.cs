using HelpDesk.Results.Extensions;
using HelpDesk.Results.Models;
using HelpDesk.Results.Services;

namespace HelpDesk.Results.Endpoints;

public static class BuisnessEndpoints
{
    public static WebApplication MapBuisnessEndpoints(this WebApplication app)
    {
        app.MapGet("/about/text", () =>
        {
            return Microsoft.AspNetCore.Http.Results.Text("Простой текст");
        });

        app.MapGet("/about/content", () =>
        {
            return Microsoft.AspNetCore.Http.Results.Content(
                "текст с явно заданным MIME-типом и кодировкой.", 
                "text/plain", 
                System.Text.Encoding.UTF8
            );
        });

        app.MapGet("/api/tickets", (ITicketRepository ticketRepository) =>
        {
            return Microsoft.AspNetCore.Http.Results.Json(ticketRepository.GetAll());
        });

        app.MapGet("/api/tickets/{id:int}", (int id, ITicketRepository ticketRepository) =>
        {
            Ticket? currentTicket = ticketRepository.GetById(id);

            if (currentTicket == null)
            {
                return Microsoft.AspNetCore.Http.Results.NotFound("Тикет не найден");
            }
            return Microsoft.AspNetCore.Http.Results.Ok(currentTicket);
        }).WithName("currentTicket");

        app.MapGet("/api/tickets/create", (string? title, int  priority, ITicketRepository ticketRepository) =>
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Microsoft.AspNetCore.Http.Results.BadRequest("title обязателен");
            }
            if (priority <= 0)
            {
                return Microsoft.AspNetCore.Http.Results.BadRequest("priority должен быть > 0");
            }

            Ticket createdTicket = ticketRepository.Create(title, priority);
            return Microsoft.AspNetCore.Http.Results.Created($"/api/tickets/{createdTicket.Id}", createdTicket);
        });

        app.MapGet("/redirect/old-tickets", () =>
        {
            return Microsoft.AspNetCore.Http.Results.LocalRedirect("/api/tickets");
        });

        app.MapGet("/redirect/ticket/{id:int}", (int id, HttpContext context) =>
        {
            return Microsoft.AspNetCore.Http.Results.RedirectToRoute("currentTicket", new { id });
        });

        app.MapGet("/files/readme", () =>
        {
            return Microsoft.AspNetCore.Http.Results.File(
                "files/readme.txt",
                "text/plain",
                "downloadName.txt"
            );
        });

        app.MapGet("/send-html", () =>
        {
            return Microsoft.AspNetCore.Http.Results.Extensions.Html(
                @"<!DOCTYPE html>
                <html>
                <head>
                    <title>О проекте</title>
                </head>
                <body>
                    <h1>HelpDesk Results API</h1>
                    <h2>Практическая работа 7-8</h2>
                    <h3>Обработка ошибок и Results API</h3>
                    <p>Этот проект демонстрирует различные типы результатов в ASP.NET Core.</p>
                    <p>Доступные маршруты можно посмотреть в таблице на главной странице.</p>
                </body>
                </html>"
            );
        });
        return app; 
    }
}