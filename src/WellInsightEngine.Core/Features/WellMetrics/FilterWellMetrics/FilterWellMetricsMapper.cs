using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Core.Features.WellMetrics.FilterWellMetrics;

public static class FilterWellMetricsMapper
{
    public static FilterWellMetricsResponse Map(IReadOnlyList<WellMetric> rows)
        => new()
        {
            Metrics = rows.Select(r => new MetricPointResponse
            {
                Time = r.Time,
                WellId = r.WellId,
                ParameterId = r.ParameterId,
                Value = r.Value
            }).ToList()
        };
}
