using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using KeeZ.Models;

namespace KeeZ.Common;

public static class ConfigManager
{
    private static readonly string AppName = Assembly.GetEntryAssembly()?.GetName().Name ?? "KeeZ";
    private static readonly byte[] EncryptionKey = GetEncryptionKey();
    private static readonly byte[] InitializationVector = new byte[16];
    public static string AppFolderPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        AppName);
    private static string SettingsFilePath => Path.Combine(
        AppFolderPath,
        "user.settings.json");
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    private static byte[] GetEncryptionKey()
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes($"{Environment.MachineName}-{AppName}"));
    }
    public static string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return plainText;

        using var aes = Aes.Create();
        aes.Key = EncryptionKey;
        aes.IV = InitializationVector;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());
    }
    public static string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return cipherText;

        try
        {
            using var aes = Aes.Create();
            aes.Key = EncryptionKey;
            aes.IV = InitializationVector;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        catch
        {
            return string.Empty; // or handle corrupt data differently
        }
    }
    
    /// <summary>
    /// Saves user settings to a JSON file in AppData
    /// </summary>
    public static void SaveSettings<T>(T settings) where T : new()
    {
        try
        {
            var settingsToSave = JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(settings));

            // Encrypt sensitive properties marked with [SensitiveData] attribute
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.GetCustomAttributes(typeof(SensitiveDataAttribute), false).Length > 0)
                {
                    var value = prop.GetValue(settingsToSave)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        prop.SetValue(settingsToSave, Encrypt(value));
                    }
                }
            }

            Directory.CreateDirectory(AppFolderPath);
            string json = JsonSerializer.Serialize(settingsToSave, JsonOptions);
            File.WriteAllText(SettingsFilePath, json);
        }
        catch (Exception ex)
        {
            // Consider logging the error
            throw new ApplicationException("Failed to save user settings", ex);
        }
    }

    /// <summary>
    /// Loads user settings from JSON file in AppData
    /// </summary>
    public static T LoadSettings<T>() where T : new()
    {
        try
        {
            if (!File.Exists(SettingsFilePath))
            {
                Directory.CreateDirectory(AppFolderPath);
                var defaultSettings = new T();
                SaveSettings(new T());
                return defaultSettings;
            }

            string json = File.ReadAllText(SettingsFilePath);
            var settings = JsonSerializer.Deserialize<T>(json) ?? new T();

            // Decrypt sensitive properties marked with [SensitiveData] attribute
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.GetCustomAttributes(typeof(SensitiveDataAttribute), false).Length > 0)
                {
                    var value = prop.GetValue(settings)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        prop.SetValue(settings, Decrypt(value));
                    }
                }
            }

            return settings;
        }
        catch (Exception ex)
        {
            // Consider logging the error
            throw new ApplicationException("Failed to load user settings", ex);
        }
    }

    /// <summary>
    /// Deletes the user settings file
    /// </summary>
    public static void ResetSettings()
    {
        try
        {
            if (File.Exists(SettingsFilePath))
            {
                File.Delete(SettingsFilePath);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to reset user settings", ex);
        }
    }
}