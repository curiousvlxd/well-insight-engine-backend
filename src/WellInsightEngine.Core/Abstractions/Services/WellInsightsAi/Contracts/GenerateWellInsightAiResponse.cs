using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.WellInsight.Payload;
using WellInsightEngine.Core.Extensions;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Abstractions.Services.WellInsightsAi.Contracts;

public sealed record GenerateWellInsightAiResponse
{
    public required WellInsightAiEnvelope Envelope { get; init; }
    public required WellInsightPayload Payload { get; init; }
    public required IReadOnlyList<WellAction> Actions { get; init; }

    public static GenerateWellInsightAiResponse Create(WellInsightAiEnvelope envelope, WellInsightPayload payload, IReadOnlyList<WellAction> actions)
        => new()
        {
            Envelope = envelope,
            Payload = payload,
            Actions = actions
        };
}

public sealed record WellInsightAiEnvelope
{
    public required string Title { get; set; }
    public required string Summary { get; set; }
    public required string[] Highlights { get; init; }
    public required string[] Suspicions { get; init; }
    public required string[] RecommendedActions { get; init; }

    public void Normalize(Well well, GroupingInterval interval)
    {
        Title = string.IsNullOrWhiteSpace(Title)
            ? string.IsNullOrWhiteSpace(well.Asset?.Name)
                ? string.Format(TitleTemplate, well.Name, interval.GetDescription())
                : string.Format(TitleWithAssetTemplate, well.Name, well.Asset!.Name.Trim(), interval.GetDescription())
            : Title.Trim();

        Summary = string.IsNullOrWhiteSpace(Summary)
            ? DefaultSummary
            : Summary.Trim();
    }
    
    private const string DefaultSummary = "Згенерований інсайт на основі агрегацій та подій.";
    private const string TitleTemplate = "Інсайт: {0} ({1})";
    private const string TitleWithAssetTemplate = "Інсайт: {0} | Ассет {1} ({2})";
}