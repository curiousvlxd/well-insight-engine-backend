namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai;

public sealed record WellInsightAiEnvelope
{
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required string[] Highlights { get; init; }
    public required string[] Suspicions { get; init; }
    public required string[] RecommendedActions { get; init; }
}