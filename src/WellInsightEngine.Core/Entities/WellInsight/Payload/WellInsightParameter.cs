using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Entities.WellInsight.Payload;

public sealed record WellInsightParameter
{
    public Guid ParameterId { get; init; }
    public string Name { get; init; }
    public ParameterDataType DataType { get; init; }
    public IReadOnlyList<WellInsightMetric> Metrics { get; init; } = [];
    public AggregationType Aggregation { get; init; }
    
    public static WellInsightParameter Create(
        Guid parameterId,
        string name,
        ParameterDataType dataType,
        IReadOnlyList<WellInsightMetric> metrics,
        AggregationType aggregation)
        => new()
        {
            ParameterId = parameterId,
            Name = name,
            DataType = dataType,
            Metrics = metrics,
            Aggregation = aggregation
        };
}