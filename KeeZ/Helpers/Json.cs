using System.Text.Json;
using System.Text.Json.Serialization;

namespace KeeZ.Helpers;

public static class Json
{
    public static T? Deserialize<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return default;
        var result = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = {
                new JsonStringEnumConverter()
            }
        });
        return result;
    }

    public static string? Serialize<T>(T obj, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        if (obj == null) return null;
        jsonSerializerOptions ??= new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {
                new JsonStringEnumConverter()
            },
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        var result = JsonSerializer.Serialize(obj, jsonSerializerOptions);
        return result;
    }

}