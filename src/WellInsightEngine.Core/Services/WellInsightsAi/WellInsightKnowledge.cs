using WellInsightEngine.Core.Enums;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Services.WellInsightsAi;

public static class WellInsightKnowledge
{
    public static readonly GroupingInterval[] IntervalsOrdered =
    [
        GroupingInterval.OneMinute,
        GroupingInterval.FiveMinutes,
        GroupingInterval.TenMinutes,
        GroupingInterval.ThirtyMinutes,
        GroupingInterval.OneHour,
        GroupingInterval.SixHours,
        GroupingInterval.TwelveHours,
        GroupingInterval.OneDay,
        GroupingInterval.OneWeek,
        GroupingInterval.OneMonth
    ];

    public static double IntervalSeconds(GroupingInterval interval)
        => interval switch
        {
            GroupingInterval.OneMinute => 60,
            GroupingInterval.FiveMinutes => 5 * 60,
            GroupingInterval.TenMinutes => 10 * 60,
            GroupingInterval.ThirtyMinutes => 30 * 60,
            GroupingInterval.OneHour => 60 * 60,
            GroupingInterval.SixHours => 6 * 60 * 60,
            GroupingInterval.TwelveHours => 12 * 60 * 60,
            GroupingInterval.OneDay => 24 * 60 * 60,
            GroupingInterval.OneWeek => 7 * 24 * 60 * 60,
            GroupingInterval.OneMonth => 30 * 24 * 60 * 60,
            _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
        };

    public static WellMetricAggregationPlan ResolvePlan(GroupingInterval interval, AggregationType[] aggregationTypes)
        => WellMetricsAggregations.Map.TryGetValue(interval, out var view)
            ? WellMetricAggregationPlan.Create(view, interval, NormalizeAggregationTypes(aggregationTypes))
            : throw new InvalidOperationException($"Unsupported interval: {interval}");

    public static AggregationType[] ResolveAggregationTypes(ParameterDataType dataType, AggregationType[] requested)
        => dataType is ParameterDataType.Categorical
            ? [AggregationType.Mode]
            : NormalizeAggregationTypes(requested);

    private static AggregationType[] NormalizeAggregationTypes(AggregationType[] aggregationTypes)
        => aggregationTypes
            .Distinct()
            .Where(x => x is AggregationType.Avg or AggregationType.Min or AggregationType.Max or AggregationType.Mode)
            .ToArray();
}