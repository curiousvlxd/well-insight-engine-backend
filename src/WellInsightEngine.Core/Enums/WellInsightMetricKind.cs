using System.ComponentModel;

namespace WellInsightEngine.Core.Enums;

public enum WellInsightMetricKind
{
    [Description("last")]
    Last,
    [Description("trend")]
    Trend
}