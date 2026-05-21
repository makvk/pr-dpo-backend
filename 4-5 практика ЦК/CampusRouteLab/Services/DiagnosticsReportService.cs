namespace CampusRouteLab.Services;

public class DiagnosticsReportService
{
    private readonly IAppInfoService _appInfo;
    private readonly IStudentCatalogService _studentCatalog;    
    private readonly IRequestContextService _requestContext;
    private readonly ITransientMarkerService _transientMarker;
    public DiagnosticsReportService(
        IAppInfoService appInfo,
        IStudentCatalogService studentCatalog,
        IRequestContextService requestContext,
        ITransientMarkerService transientMarker)
    {
        _appInfo = appInfo;
        _studentCatalog = studentCatalog;
        _requestContext = requestContext;
        _transientMarker = transientMarker; 
    }
    public Guid AppInstanceId => _appInfo.AppInstanceId;
    public Guid RequestId => _requestContext.RequestId;
    public Guid TransientMarkerId => _transientMarker.MarkerId;
}