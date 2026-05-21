namespace CampusRouteLab.Services;

public class AppInfoService : IAppInfoService
{
    public Guid AppInstanceId { get; }
    public DateTime StartedAt { get; }

    public AppInfoService()
    {
        AppInstanceId = Guid.NewGuid();
        StartedAt = DateTime.Now;
    }
}