using System.Globalization;
using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Entities.WellInsight.Payload;

public sealed record WellInsightKpiGroup
{
    public required Guid ParameterId { get; init; }
    public required string Name { get; init; }
    public AggregationType Aggregation { get; init; }
    public required IReadOnlyList<WellInsightKpiItem> Items { get; init; }
    
    public static WellInsightKpiGroup Create(Guid parameterId, string name, AggregationType aggregation, IReadOnlyList<WellInsightKpiItem> kpis)
        => new()
        {
            ParameterId = parameterId,
            Name = name,
            Aggregation = aggregation,
            Items = kpis
        };
    
    public static IReadOnlyList<WellInsightKpiGroup> BuildKpis(IEnumerable<WellInsightAggregation> aggregations)
    {
        var result = new List<WellInsightKpiGroup>();

        foreach (var g in aggregations)
        {
            foreach (var p in g.Parameters)
            {
                if (p.DateTicks.Count == 0)
                    continue;

                var firstRaw = p.DateTicks[0].Value;
                var lastRaw = p.DateTicks[^1].Value;
                var items = new List<WellInsightKpiItem>
                {
                    WellInsightKpiItem.Create(WellInsightMetricKind.Last, lastRaw)
                };

                if (g.DataType != ParameterDataType.Categorical &&
                    decimal.TryParse(firstRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out var first) &&
                    decimal.TryParse(lastRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out var last) &&
                    first != 0m)
                {
                    var trendPercent = (last - first) / Math.Abs(first) * 100m;
                    items.Add(WellInsightKpiItem.Create(
                        WellInsightMetricKind.Trend,
                        $"{trendPercent:+0.0;-0.0}%"));
                }

                result.Add(Create(p.ParameterId, p.ParameterName, g.Aggregation, items));
            }
        }

        return result;
    }

}

public sealed record WellInsightKpiItem
{
    public WellInsightMetricKind Kind { get; init; }
    public required string Value { get; init; }

    public static WellInsightKpiItem Create(WellInsightMetricKind kind, string value)
        => new()
        {
            Kind = kind,
            Value = value
        };
}