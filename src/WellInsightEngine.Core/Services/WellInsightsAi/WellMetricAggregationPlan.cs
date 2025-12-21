using WellInsightEngine.Core.Enums;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Services.WellInsightsAi;

public sealed record WellMetricAggregationPlan
{
    public required string View { get; init; }
    public required GroupingInterval GroupingInterval { get; init; }
    public required AggregationType[] Aggregations { get; init; }

    public static WellMetricAggregationPlan Create(string view, GroupingInterval groupingInterval, AggregationType[] aggregations)
        => new()
        {
            View = view,
            GroupingInterval = groupingInterval,
            Aggregations = aggregations
        };
}