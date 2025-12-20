namespace WellInsightEngine.Core.Entities.WellInsight.Payload;

public sealed record WellInsightMetric
{
    public DateTimeOffset Timestamp { get; init; }
    public string Value { get; init; }
    
    public static WellInsightMetric Create(
        DateTimeOffset timestamp,
        string value)
        => new()
        {
            Timestamp = timestamp,
            Value = value
        };
}