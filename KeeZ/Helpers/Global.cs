using System.IO;

namespace KeeZ.Helpers;

public static class Global
{
    public static string StartUpPath { get; set; } = AppContext.BaseDirectory;
    public static string Absolute(string relativePath)
    {
        return Path.Combine(StartUpPath, relativePath);
    }
    public static async Task<string> ReadAllTextIfExist(string relativePath)
    {
        var path = Absolute(relativePath);
        if (File.Exists(path)) 
            return await File.ReadAllTextAsync(path).ConfigureAwait(false);
        return "";
    }
    public static async Task WriteAllText(string folder, string fileName, string fileContent)
    {
        var path = Absolute(folder);
        var fullPath = Path.Combine(path, fileName);
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
        await File.WriteAllTextAsync(fullPath, fileContent).ConfigureAwait(false);
    }
}