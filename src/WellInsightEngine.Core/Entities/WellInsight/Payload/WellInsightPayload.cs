namespace WellInsightEngine.Core.Entities.WellInsight.Payload;

public sealed record WellInsightPayload
{
    public IReadOnlyList<WellInsightParameter> Parameters { get; init; }
    public IReadOnlyList<WellInsightKpi> Kpis { get; init; }
    
    public static WellInsightPayload Create(IReadOnlyList<WellInsightParameter> series, IReadOnlyList<WellInsightKpi> kpis)
        => new()
        {
            Parameters = series,
            Kpis = kpis
        };
}