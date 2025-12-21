using System.ComponentModel;
using System.Reflection;

namespace WellInsightEngine.Core.Extensions;

public static class EnumDescriptionExtensions
{
    public static string? GetDescription(this Enum value)
    {
        var type = value.GetType();
        var name = value.ToString();
        var field = type.GetField(name, BindingFlags.Public | BindingFlags.Static);
        return field?.GetCustomAttribute<DescriptionAttribute>()?.Description;
    }
}