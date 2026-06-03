namespace HelpDesk.Results.Results;

public sealed class HtmlResult(string html) : IResult
{
    // private readonly string _htmlCode = html ?? "";
    public async Task ExecuteAsync(HttpContext context)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync(html);
    }
}
