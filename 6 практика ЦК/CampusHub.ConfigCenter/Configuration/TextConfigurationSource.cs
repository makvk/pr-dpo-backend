namespace CampusHub.ConfigCenter.Configuration;

public class TextConfigurationSource(string filename) : IConfigurationSource
{
    private readonly string _fileName = filename;

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        string filePath = builder.GetFileProvider().GetFileInfo(_fileName).PhysicalPath ?? "";
        return new TextConfigurationProvider(filePath);
    }
}