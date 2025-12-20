using WellInsightEngine.Core.Entities.WellInsight;
using WellInsightEngine.Core.Entities.WellInsight.Payload;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight;

public sealed class GenerateWellInsightResponse
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
    public required WellInsightPayload Payload { get; init; }
    public required IReadOnlyList<string> Highlights { get; init; }
    public required IReadOnlyList<string> Suspicions { get; init; }
    public required IReadOnlyList<string> RecommendedActions { get; init; }
    
    public static GenerateWellInsightResponse Create(WellInsight insight)
        => new()
        {   
            Interval = insight.Interval,
            InsightId = insight.Id,
            WellId = insight.WellId,
            CreatedAt = insight.CreatedAt,
            From = insight.From,
            To = insight.To,
            Title = insight.Title,
            Summary = insight.Summary,
            Payload = insight.Payload,
            Highlights = insight.Highlights,
            Suspicions = insight.Suspicions,
            RecommendedActions = insight.RecommendedActions,
            Slug = insight.Slug
        };
}