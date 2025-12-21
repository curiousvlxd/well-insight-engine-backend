using WellInsightEngine.Core.Entities.WellInsight;
using WellInsightEngine.Core.Features.Common.Pagination;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Features.WellInsights.FilterWellInsights;

public sealed record FilterWellInsightsResponse
{
    public required OffsetPagedList<WellInsightItemResponse> Insights { get; init; }

    public static FilterWellInsightsResponse Create(OffsetPagedList<WellInsightItemResponse> insights)
        => new()
        {
            Insights = insights
        };
}

public sealed record WellInsightItemResponse
{
    public required Guid InsightId { get; init; }
    public required Guid WellId { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset From { get; init; }
    public required DateTimeOffset To { get; init; }
    public required GroupingInterval Interval { get; init; }
    public required string Title { get; init; }
    public required string Slug { get; init; }
    public required string Summary { get; init; }
    public required IReadOnlyList<string> Highlights { get; init; }

    public static WellInsightItemResponse Create(WellInsight insight)
        => new()
        {
            InsightId = insight.Id,
            WellId = insight.WellId,
            CreatedAt = insight.CreatedAt,
            From = insight.From,
            To = insight.To,
            Interval = insight.Interval,
            Title = insight.Title,
            Slug = insight.Slug,
            Summary = insight.Summary,
            Highlights = insight.Highlights
        };
}