namespace CampusRouteLab.Services;

public class TransientMarkerService : ITransientMarkerService
{
    public Guid MarkerId { get; }
    public DateTime MarkerTime { get; }

    public TransientMarkerService()
    {
        MarkerId = Guid.NewGuid();
        MarkerTime = DateTime.Now;
    }
}