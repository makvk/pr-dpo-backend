namespace CampusRouteLab.Services;

public class RequestContextService : IRequestContextService
{
    public Guid RequestId { get; }
    public DateTime CreatedAt { get; }
    public RequestContextService()
    {
        RequestId = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }
}