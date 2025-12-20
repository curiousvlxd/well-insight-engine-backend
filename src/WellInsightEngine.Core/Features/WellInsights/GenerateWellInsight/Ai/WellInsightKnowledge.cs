using System.Globalization;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Enums;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai;

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
            GroupingInterval.OneYear => 365 * 24 * 60 * 60,
            _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
        };

    public static string ResolveAggregate(GroupingInterval interval)
        => WellMetricsAggregations.Map.TryGetValue(interval, out var view)
            ? view
            : throw new InvalidOperationException($"Unsupported interval: {interval}");

    public static AggregationType DefaultAggregation(ParameterDataType type) => type switch
    {
        ParameterDataType.Categorical => AggregationType.Mode,
        _ => AggregationType.Avg
    };
    
    public static AggregationType ResolveAggregation(ParameterDataType dataType, AggregationType requested)
        => dataType switch
        {
            ParameterDataType.Categorical => AggregationType.Mode,
            _ => requested is AggregationType.Min or AggregationType.Max ? requested : AggregationType.Avg
        };



    public static string FormatValue(ParameterDataType dataType, AggregationType aggregation, WellMetricAggregation row)
        => (dataType, aggregation) switch
        {
            (ParameterDataType.Categorical, _) => row.ModeValue ?? string.Empty,
            (_, AggregationType.Mode) => row.ModeValue ?? string.Empty,
            (_, AggregationType.Min) => ToInvariant(row.MinValue),
            (_, AggregationType.Max) => ToInvariant(row.MaxValue),
            _ => ToInvariant(row.AvgValue)
        };

    private static string ToInvariant(decimal value) => value.ToString(CultureInfo.InvariantCulture);
}