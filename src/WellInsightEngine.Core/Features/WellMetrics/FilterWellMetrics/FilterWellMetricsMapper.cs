using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Core.Features.WellMetrics.FilterWellMetrics;

public static class FilterWellMetricsMapper
{
    public static FilterWellMetricsResponse Map(IReadOnlyList<WellMetric> rows, IReadOnlyDictionary<Guid, string> parameterNames)
        => new()
        {
            Series = rows
                .GroupBy(x => new { x.WellId, x.ParameterId })
                .Select(g => new MetricSeriesResponse
                {
                    WellId = g.Key.WellId,
                    ParameterId = g.Key.ParameterId,
                    ParameterName = parameterNames.TryGetValue(g.Key.ParameterId, out var name) ? name : string.Empty,
                    DateTicks = g
                        .OrderBy(x => x.Time)
                        .Select(x => new MetricTickResponse
                        {
                            Time = x.Time,
                            Value = x.Value
                        })
                        .ToList()
                })
                .ToList()
        };
}