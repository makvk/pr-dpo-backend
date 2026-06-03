using HelpDesk.Results.Services;

namespace HelpDesk.Results.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddTicketServices(this IServiceCollection services)
    {
        services.AddSingleton<ITicketRepository, InMemoryTicketRepository>();
        return services;
    }
}