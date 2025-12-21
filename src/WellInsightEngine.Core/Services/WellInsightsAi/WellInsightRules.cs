using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Services.WellInsightsAi;

public static class WellInsightRules
{
    public static GroupingInterval ChooseInterval(DateTimeOffset fromUtc, DateTimeOffset toUtc, int maxMetrics)
    {
        var seconds = (toUtc - fromUtc).TotalSeconds;

        foreach (var interval in WellInsightKnowledge.IntervalsOrdered)
        {
            var bucketSeconds = WellInsightKnowledge.IntervalSeconds(interval);
            
            if (seconds / bucketSeconds <= maxMetrics)
                return interval;
        }

        return GroupingInterval.OneYear;
    }
}