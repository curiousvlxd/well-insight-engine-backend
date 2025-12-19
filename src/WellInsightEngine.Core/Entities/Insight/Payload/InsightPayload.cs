namespace WellInsightEngine.Core.Entities.Insight.Payload;

public sealed record InsightPayload
{
    public IReadOnlyList<InsightSeries> Series { get; init; }
    public IReadOnlyList<InsightEvent> Events { get; init; }
    public IReadOnlyList<InsightKpi> Kpis { get; init; }
}