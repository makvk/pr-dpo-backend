namespace CampusHub.ConfigCenter.Configuration;
public static class TextConfigurationExtensions
{
    public static IConfigurationBuilder AddTextFile(this IConfigurationBuilder builder, string path)
    {
        ArgumentNullException.ThrowIfNull(builder);
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("Путь к файлу не найден");
        }
        var source = new TextConfigurationSource(path);
        builder.Add(source);
        return builder;
    }
}