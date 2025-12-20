namespace WellInsightEngine.Core.Features.WellMetrics;

public static class WellMetricsAggregations
{
    public static readonly IReadOnlyDictionary<GroupingInterval, string> Map =
        new Dictionary<GroupingInterval, string>
        {
            [GroupingInterval.OneMinute] = "well_metrics_aggregate_1m",
            [GroupingInterval.FiveMinutes] = "well_metrics_aggregate_5m",
            [GroupingInterval.TenMinutes] = "well_metrics_aggregate_10m",
            [GroupingInterval.ThirtyMinutes] = "well_metrics_aggregate_30m",
            [GroupingInterval.OneHour] = "well_metrics_aggregate_1h",
            [GroupingInterval.SixHours] = "well_metrics_aggregate_6h",
            [GroupingInterval.TwelveHours] = "well_metrics_aggregate_12h",
            [GroupingInterval.OneDay] = "well_metrics_aggregate_1d",
            [GroupingInterval.OneWeek] = "well_metrics_aggregate_1w",
            [GroupingInterval.OneMonth] = "well_metrics_aggregate_1mo",
            [GroupingInterval.OneYear] = "well_metrics_aggregate_1y"
        };
}