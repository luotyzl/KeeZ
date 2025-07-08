namespace KeeZ.Models;

public class UserConfiguration
{
    public string LastOpenedFilePath { get; set; } = "";
    public string WebDavPath { get; set; } = "";
    public string WebDavUser { get; set; } = "";
    [SensitiveData]
    public string WebDavPassword { get; set; } = "";
}