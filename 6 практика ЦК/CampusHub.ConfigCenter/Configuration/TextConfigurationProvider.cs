using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CampusHub.ConfigCenter.Configuration;

public class TextConfigurationProvider(string filePath) : ConfigurationProvider
{
    private readonly string _filePath = filePath;

    public override void Load()
    {
        var data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        using (StreamReader textReader = new StreamReader(_filePath))
        {
            string? line;
            while ((line = textReader.ReadLine()) != null)
            {
                string key = line.Trim();
                string value = textReader.ReadLine() ?? string.Empty;
                data.Add(key, value);
            }
        }
        this.Data = data;
    }
}