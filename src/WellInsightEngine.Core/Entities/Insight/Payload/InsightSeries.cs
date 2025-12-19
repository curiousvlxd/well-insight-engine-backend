using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Entities.Insight.Payload;

public sealed record InsightSeries
{
    public Guid ParameterId { get; init; }
    public string Name { get; init; }
    public ParameterDataType DataType { get; init; }
    public IReadOnlyList<InsightPoint> Points { get; init; }
    public AggregationType Aggregation { get; init; }
}