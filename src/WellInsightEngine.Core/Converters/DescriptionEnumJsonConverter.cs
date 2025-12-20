namespace WellInsightEngine.Core.Converters;

using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class DescriptionEnumJsonConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    private static readonly Dictionary<string, TEnum> FromDescription =
        Enum.GetValues<TEnum>()
            .Select(v => new
            {
                Value = v,
                Description = typeof(TEnum)
                    .GetField(v.ToString())?
                    .GetCustomAttribute<DescriptionAttribute>()?
                    .Description
            })
            .Where(x => x.Description is not null)
            .ToDictionary(x => x.Description!, x => x.Value, StringComparer.OrdinalIgnoreCase);

    private static readonly Dictionary<TEnum, string> ToDescription =
        Enum.GetValues<TEnum>()
            .ToDictionary(
                v => v,
                v => typeof(TEnum)
                         .GetField(v.ToString())?
                         .GetCustomAttribute<DescriptionAttribute>()?
                         .Description
                     ?? v.ToString()
            );

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (value is null)
            throw new JsonException($"Null is not valid for enum {typeof(TEnum).Name}");

        if (FromDescription.TryGetValue(value, out var enumValue))
            return enumValue;

        if (Enum.TryParse<TEnum>(value, true, out var parsed))
            return parsed;

        throw new JsonException($"Unknown value '{value}' for enum {typeof(TEnum).Name}");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(ToDescription[value]);
    }
}