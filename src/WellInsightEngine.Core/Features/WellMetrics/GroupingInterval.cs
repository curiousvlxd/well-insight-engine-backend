using System.ComponentModel;
using System.Text.Json.Serialization;
using WellInsightEngine.Core.Converters;

namespace WellInsightEngine.Core.Features.WellMetrics;

[JsonConverter(typeof(DescriptionEnumJsonConverter<GroupingInterval>))]
public enum GroupingInterval
{
    [Description("1m")]
    OneMinute,

    [Description("5m")]
    FiveMinutes,

    [Description("10m")]
    TenMinutes,

    [Description("30m")]
    ThirtyMinutes,

    [Description("1h")]
    OneHour,

    [Description("6h")]
    SixHours,

    [Description("12h")]
    TwelveHours,

    [Description("1d")]
    OneDay,

    [Description("7d")]
    OneWeek,

    [Description("1mo")]
    OneMonth,

    [Description("1y")]
    OneYear
}
