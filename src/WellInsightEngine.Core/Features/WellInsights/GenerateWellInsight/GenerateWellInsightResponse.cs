using WellInsightEngine.Core.Entities.Insight.Payload;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight;

public sealed class GenerateWellInsightResponse
{
    public required Guid InsightId { get; init; }
    public required Guid WellId { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset From { get; init; }
    public required DateTimeOffset To { get; init; }
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required InsightPayload Payload { get; init; }
    public required IReadOnlyList<string> Highlights { get; init; }
    public required IReadOnlyList<string> Suspicions { get; init; }
    public required IReadOnlyList<string> RecommendedActions { get; init; }
}