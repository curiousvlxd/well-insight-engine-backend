using System.ComponentModel;

namespace WellInsightEngine.Core.Enums;

public enum AggregationType
{   
    [Description("avg")]
    Avg = 1,
    [Description("min")]
    Min = 2,
    [Description("max")]
    Max = 3,
    [Description("mode")]
    Mode = 7
}