namespace CampusHub.ConfigCenter.Configuration;

public class TextConfigurationSource : IConfigurationSource
{
    private readonly string _fileName;
    public TextConfigurationSource(string filename)
    {
        _fileName = filename;
    }
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        string filePath = builder.GetFileProvider().GetFileInfo(_fileName).PhysicalPath ?? "";
        return new TextConfigurationProvider(filePath);
    }
}