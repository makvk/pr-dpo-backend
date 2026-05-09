namespace StudentPortal.Diagnostics.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        if (context.Response.StatusCode == 403)
        {
            await context.Response.WriteAsync("\n Error middleware: Access denied");
        } 
        else if (context.Response.StatusCode == 404)
        {
            await context.Response.WriteAsync("\n Error middleware: Page not found");
        }
    } 
}