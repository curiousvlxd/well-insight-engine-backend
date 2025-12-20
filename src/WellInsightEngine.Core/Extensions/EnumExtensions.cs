using System.ComponentModel;
using System.Reflection;

namespace WellInsightEngine.Core.Extensions;

public static class EnumDescriptionExtensions
{
    public static string? GetDescription<TEnum>(this TEnum value) where TEnum : struct, Enum
    {
        var name = value.ToString();
        var field = typeof(TEnum).GetField(name, BindingFlags.Public | BindingFlags.Static);
        return field?.GetCustomAttribute<DescriptionAttribute>()?.Description;
    }
}