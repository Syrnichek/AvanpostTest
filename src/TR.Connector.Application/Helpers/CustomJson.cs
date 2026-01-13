using System.Text.Json;
using System.Text.Json.Serialization;

namespace TR.Connector.Application.Helpers;

public class CustomJson
{
    private static JsonSerializerOptions Options => new()
    {
        TypeInfoResolver = CustomJsonContext.Default,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };
    
    public static string? Serialize<T>(T? value)
    {
        if (value == null)
        {
            return null;
        }

        return JsonSerializer.Serialize(value, Options);
    }
    
    public static T DeserializeRequired<T>(string json, Type type)
        where T : class
    {
        ArgumentException.ThrowIfNullOrEmpty(json);

        return (T?)JsonSerializer.Deserialize(json, type, Options)
               ?? throw new InvalidOperationException($"Cannot deserialize to {typeof(T).FullName}");
    }
}