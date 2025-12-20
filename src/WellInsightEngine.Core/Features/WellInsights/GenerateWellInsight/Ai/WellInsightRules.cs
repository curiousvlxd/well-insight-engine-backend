using WellInsightEngine.Core.Enums;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai;

public static class WellInsightRules
{
    public static AggregationType ResolveAggregation(ParameterDataType type)
        => type == ParameterDataType.Categorical
            ? AggregationType.Mode
            : AggregationType.Avg;

    public static GroupingInterval ChooseInterval(
        DateTimeOffset fromUtc,
        DateTimeOffset toUtc,
        int maxPoints)
    {
        var seconds = (toUtc - fromUtc).TotalSeconds;

        if (Points(60) <= maxPoints) return GroupingInterval.OneMinute;
        if (Points(300) <= maxPoints) return GroupingInterval.FiveMinutes;
        if (Points(600) <= maxPoints) return GroupingInterval.TenMinutes;
        if (Points(1800) <= maxPoints) return GroupingInterval.ThirtyMinutes;
        if (Points(3600) <= maxPoints) return GroupingInterval.OneHour;
        if (Points(21600) <= maxPoints) return GroupingInterval.SixHours;
        if (Points(43200) <= maxPoints) return GroupingInterval.TwelveHours;
        if (Points(86400) <= maxPoints) return GroupingInterval.OneDay;
        if (Points(604800) <= maxPoints) return GroupingInterval.OneWeek;

        return seconds / 2_592_000 <= maxPoints
            ? GroupingInterval.OneMonth
            : GroupingInterval.OneYear;

        double Points(double bucket) => seconds / bucket;
    }
}