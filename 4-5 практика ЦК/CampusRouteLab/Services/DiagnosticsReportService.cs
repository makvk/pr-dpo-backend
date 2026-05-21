namespace CampusRouteLab.Services;

public class DiagnosticsReportService
{
    public readonly IAppInfoService appInfo; 
    public readonly IRequestContextService requestContext;
    public readonly ITransientMarkerService transientMarker;
    public DiagnosticsReportService(
        IAppInfoService appInfo,
        IRequestContextService requestContext,
        ITransientMarkerService transientMarker
    )
    {
        this.appInfo = appInfo;
        this.requestContext = requestContext;
        this.transientMarker = transientMarker; 
    }
}