namespace WellInsightEngine.Core.Entities.Insight.Payload;

public sealed record InsightKpi
{
    public string Code { get; init; }
    public string Label { get; init; }
    public string Value { get; init; }
    public string? Delta { get; init; }
}