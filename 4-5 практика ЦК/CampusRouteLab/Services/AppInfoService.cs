namespace CampusRouteLab.Services;

public class AppInfoService : IAppInfoService
{
    public Guid AppInstanceId => Guid.NewGuid();
    public DateTime StartedAt => DateTime.Now;
}