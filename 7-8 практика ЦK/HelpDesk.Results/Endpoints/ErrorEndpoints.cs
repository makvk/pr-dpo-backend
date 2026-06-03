using static Microsoft.AspNetCore.Http.Results;

namespace HelpDesk.Results.Endpoints;

public static class ErrorEndpoints
{
    public static WebApplication MapErrorEndpoints(this WebApplication app)
    {
        app.Map("/throw", () =>
        {
            throw new Exception("Произошла ошибка!");
        });

        app.Map("/error/exception", () =>
        {
            return Microsoft.AspNetCore.Http.Results.StatusCode(500);
        });

        app.MapGet("error/status/{statusCode:int}", (int statusCode) =>
        {
            string description = statusCode switch
            {
                // 4xx - Ошибки клиента
                400 => "Bad Request - Неверный запрос",
                401 => "Unauthorized - Не авторизован",
                403 => "Forbidden - Доступ запрещён",
                404 => "Not Found - Ресурс не найден",
                405 => "Method Not Allowed - Метод не разрешён",
                408 => "Request Timeout - Таймаут запроса",
                409 => "Conflict - Конфликт данных",
                410 => "Gone - Ресурс удалён",
                413 => "Payload Too Large - Запрос слишком большой",
                415 => "Unsupported Media Type - Неподдерживаемый тип данных",
                422 => "Unprocessable Entity - Невалидные данные",
                429 => "Too Many Requests - Слишком много запросов",
                
                // 5xx - Ошибки сервера
                500 => "Internal Server Error - Внутренняя ошибка сервера",
                501 => "Not Implemented - Функционал не реализован",
                502 => "Bad Gateway - Неверный ответ шлюза",
                503 => "Service Unavailable - Сервис недоступен",
                504 => "Gateway Timeout - Таймаут шлюза",
                505 => "HTTP Version Not Supported - Версия HTTP не поддерживается",
                
                >= 400 and < 500 => $"Client Error - Ошибка клиента (код {statusCode})",
                >= 500 and < 600 => $"Server Error - Ошибка сервера (код {statusCode})",
                
                _ => "Unknown Status Code - Неизвестный статус-код"
            };
            
            var message = $"Error {statusCode}: {description}";
            
            return Microsoft.AspNetCore.Http.Results.Text(message, statusCode: statusCode);
        });

        app.MapGet("/status/unauthorized", () =>
        {
            return Microsoft.AspNetCore.Http.Results.Unauthorized();
        });

        app.MapGet("/status/forbidden", () =>
        {
            return Microsoft.AspNetCore.Http.Results.StatusCode(403);
        });

        app.MapGet("/status/custom/{statusCode:int}", (int statusCode) =>
        {
            return Microsoft.AspNetCore.Http.Results.StatusCode(statusCode);
        });

        app.MapGet("/unknown", () =>
        {
            return Microsoft.AspNetCore.Http.Results.NotFound("Ресурс не найден");
        });

        return app;
    }
}