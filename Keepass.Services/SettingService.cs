using System.Text.Json;
using Keepass.Common;
using Keepass.Models;

namespace Keepass.Services;

public class SettingService(Serilog.ILogger logger)
{
    private const string FileName = "user-settings.json";
    private readonly string _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);
    private UserSettings _settings = new UserSettings();
    
    public async Task<UserSettings> Load()
    {
        if (File.Exists(_settingsPath))
        {
            try
            {
                var json = await File.ReadAllTextAsync(_settingsPath).ConfigureAwait(false);
                _settings = Json.Deserialize<UserSettings>(json) ?? new UserSettings();
            }
            catch
            {
                _settings = new UserSettings();
            }
        }
        else
        {
            _settings = new UserSettings(); // first run
        }

        return _settings;
    }

    public async Task Save()
    {
        var json = Json.Serialize(_settings);
        await File.WriteAllTextAsync(_settingsPath, json).ConfigureAwait(false);
    } 
}