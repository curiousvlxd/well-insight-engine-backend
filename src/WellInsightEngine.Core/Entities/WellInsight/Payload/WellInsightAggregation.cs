using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Entities.WellInsight.Payload;

public sealed record WellInsightAggregation
{
    public required ParameterDataType DataType { get; init; }
    public required AggregationType Aggregation { get; init; }
    public required IReadOnlyList<WellInsightParameter> Parameters { get; init; }

    public static WellInsightAggregation Create(ParameterDataType dataType, AggregationType aggregation, IReadOnlyList<WellInsightParameter> parameters)
        => new()
        {
            DataType = dataType,
            Aggregation = aggregation,
            Parameters = parameters
        };
}