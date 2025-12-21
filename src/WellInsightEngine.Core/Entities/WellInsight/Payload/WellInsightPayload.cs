namespace WellInsightEngine.Core.Entities.WellInsight.Payload;

public sealed record WellInsightPayload
{
    public required IReadOnlyList<WellInsightAggregation> Aggregations { get; init; }
    public required IReadOnlyList<WellInsightKpi> Kpis { get; init; }

    public static WellInsightPayload Create(IEnumerable<WellInsightAggregation> aggregations)
    {
        var items = aggregations.ToList();
        return new WellInsightPayload
        {
            Aggregations = items,
            Kpis = WellInsightKpi.BuildKpis(items)
        };
    }
}