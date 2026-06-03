using HelpDesk.Results.Results;

namespace HelpDesk.Results.Extensions;

public static class HtmlResultExtension
{
public static IResult Html(this IResultExtensions extensions, string html)
    {
        return new HtmlResult(html);
    }
}