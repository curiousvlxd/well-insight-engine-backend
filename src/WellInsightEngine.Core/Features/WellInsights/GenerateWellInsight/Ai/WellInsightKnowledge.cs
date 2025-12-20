using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai;

public static class WellInsightKnowledge
{
    public static string HumanAggregation(AggregationType type) =>
        type switch
        {
            AggregationType.Avg => "average value",
            AggregationType.Min => "minimum value",
            AggregationType.Max => "maximum value",
            AggregationType.Sum => "total value",
            AggregationType.Mode => "most frequent state",
            _ => "value"
        };

    public static string HumanDataType(ParameterDataType type) =>
        type switch
        {
            ParameterDataType.Numeric => "numeric sensor",
            ParameterDataType.Categorical => "state sensor",
            _ => "sensor"
        };

    public static bool SupportsAggregation(ParameterDataType type, AggregationType aggregation)
        => type is not ParameterDataType.Categorical || aggregation is AggregationType.Mode;
}