namespace WellInsightEngine.Core.Entities.Insight.Payload;

public sealed record InsightPoint
{
    public DateTimeOffset Timestamp { get; init; }
    public string Value { get; init; }
}