namespace CampusRouteLab.Services;

public class TransientMarkerService : ITransientMarkerService
{
    public Guid MarkerId => Guid.NewGuid();
}