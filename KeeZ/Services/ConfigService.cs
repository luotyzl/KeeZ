using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Windows;
using KeeZ.Helpers;
using KeeZ.Models;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace KeeZ.Services;

public class ConfigService(Serilog.ILogger logger)
{
    public static UserConfiguration? Config { get; set; }

    public async Task<UserConfiguration> Get()
    {
        return Config ??= await Read().ConfigureAwait(false);
    }

    public async Task Save()
    {
        await Write(Config);
    }
    private const string ConfigFileFolder = "User";
    private const string ConfigFile = "config.json";
    private string ConfigFilePath => $"{ConfigFileFolder}/{ConfigFile}";

    public async Task<UserConfiguration> Read()
    {
        try
        {
            var json = await Global.ReadAllTextIfExist(ConfigFilePath).ConfigureAwait(false);
            var config = Json.Deserialize<UserConfiguration>(json);
            if (config == null)
            {
                return new UserConfiguration();
            }
            Config = config;
            return config;
        }
        catch (Exception e)
        {
            logger.Error(e, "Failed to read user config from {path}", ConfigFilePath);
            return new UserConfiguration();
        }
    }

    public async Task Write(UserConfiguration? config)
    {
        try
        {
            await Global.WriteAllText(ConfigFileFolder, ConfigFile, Json.Serialize(config) ?? "");
        }
        catch (Exception e)
        {
            logger.Error(e, "Failed to write user config to {path}", ConfigFilePath);
        }
    }
}
