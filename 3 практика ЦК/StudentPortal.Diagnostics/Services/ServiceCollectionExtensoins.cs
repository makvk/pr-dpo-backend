namespace StudentPortal.Diagnostics.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStudentPortalServices(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddTransient<IGenerateService, GenerateService>();
        return services;
    }
}