namespace WellInsightEngine.Core.Entities;

public sealed record WellMetricRow
{
    public Guid WellId { get; init; }
    public Guid ParameterId { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public string Value { get; init; } = string.Empty;
}
