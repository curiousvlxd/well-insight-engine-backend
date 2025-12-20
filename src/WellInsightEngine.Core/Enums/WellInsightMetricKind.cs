using System.ComponentModel;

namespace WellInsightEngine.Core.Enums;

public enum WellInsightMetricKind
{
    [Description("last")]
    Last,
    [Description("avg")]
    Average,
    [Description("min")]
    Min,
    [Description("max")]
    Max,
    [Description("trend")]
    Trend
}