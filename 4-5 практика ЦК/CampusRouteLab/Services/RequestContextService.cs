namespace CampusRouteLab.Services;

public class RequestContextService : IRequestContextService
{
    public Guid RequestId => Guid.NewGuid();
    public DateTime CreatedAt => DateTime.Now;
}