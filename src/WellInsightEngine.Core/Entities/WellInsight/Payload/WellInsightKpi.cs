using System.Globalization;
using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Entities.WellInsight.Payload;

public sealed record WellInsightKpi
{
    public required Guid ParameterId { get; init; }
    public WellInsightMetricKind Kind { get; init; }
    public AggregationType Aggregation { get; init; }
    public required string Name { get; init; }
    public required string Value { get; init; }
    public string? Change { get; init; }

    public static WellInsightKpi Create(Guid parameterId, AggregationType aggregation, WellInsightMetricKind kind, string name, string value, string? change)
        => new()
        {
            ParameterId = parameterId,
            Aggregation = aggregation,
            Kind = kind,
            Name = name,
            Value = value,
            Change = change
        };
    
    public static IReadOnlyList<WellInsightKpi> BuildKpis(IEnumerable<WellInsightAggregation> aggregations)
    {
        var result = new List<WellInsightKpi>();

        foreach (var g in aggregations)
        {
            foreach (var p in g.Parameters)
            {
                if (p.DateTicks.Count == 0)
                    continue;

                var first = p.DateTicks[0].Value;
                var last = p.DateTicks[^1].Value;

                string? change = null;

                if (g.DataType is not ParameterDataType.Categorical
                    && decimal.TryParse(first, NumberStyles.Any, CultureInfo.InvariantCulture, out var f)
                    && decimal.TryParse(last, NumberStyles.Any, CultureInfo.InvariantCulture, out var l))
                    change = (l - f).ToString(CultureInfo.InvariantCulture);

                result.Add(Create(p.ParameterId, g.Aggregation, WellInsightMetricKind.Last, p.ParameterName, last, change));
            }
        }

        return result;
    }
}