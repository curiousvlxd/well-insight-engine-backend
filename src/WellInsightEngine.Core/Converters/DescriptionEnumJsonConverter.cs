using System.Text.Json;
using System.Text.Json.Serialization;
using WellInsightEngine.Core.Extensions;

public sealed class DescriptionEnumJsonConverter : JsonConverter<Enum>
{
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsEnum;

    public override Enum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var raw = reader.GetString()!;

        foreach (var value in Enum.GetValues(typeToConvert))
        {
            var enumValue = (Enum)value;

            if (string.Equals(enumValue.GetDescription(), raw, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(enumValue.ToString(), raw, StringComparison.OrdinalIgnoreCase))
            {
                return enumValue;
            }
        }

        throw new JsonException($"Invalid value '{raw}' for enum {typeToConvert.Name}");
    }

    public override void Write(Utf8JsonWriter writer, Enum value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.GetDescription() ?? value.ToString());
}