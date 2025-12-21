using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Features.WellInsights.GenerateWellInsight.Ai.Options;

public sealed record WellInsightsAiOptions
{
    public AggregationType[] AggregationTypes { get; init; } =
    [
        AggregationType.Avg,
        AggregationType.Min,
        AggregationType.Max,
        AggregationType.Mode
    ];

    public int MaxPointsPerSeries { get; init; } 
    public int MaxActionsPerPrompt  { get; init; }
    public int TopKSeriesForPrompt { get; init; }
    public int PromptMaxChars { get; init; }
    public double SpikeZScore { get; init; }
    public double ChangePctThreshold { get; init; }
}