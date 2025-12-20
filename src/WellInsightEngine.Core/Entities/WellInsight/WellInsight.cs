using WellInsightEngine.Core.Entities.WellInsight.Payload;

namespace WellInsightEngine.Core.Entities.WellInsight;

public sealed class WellInsight
{
    public Guid Id { get; init; }
    public Guid WellId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public required WellInsightPayload Payload { get; set; }
    public string[] Highlights { get; init; } = [];
    public string[] Suspicions { get; init; } = [];
    public string[] RecommendedActions { get; init; } = [];
    
    public static WellInsight Create(Guid wellId, DateTimeOffset fromUtc, DateTimeOffset toUtc, string title, string summary, string[] highlights, string[] suspicions, string[] recommendedActions, WellInsightPayload payload)
        => new()
        {
            Id = Guid.NewGuid(),
            WellId = wellId,
            CreatedAt = DateTimeOffset.UtcNow,
            From = fromUtc,
            To = toUtc,
            Title = title,
            Summary = summary,
            Payload = payload,
            Highlights = highlights,
            Suspicions = suspicions,
            RecommendedActions = recommendedActions,
        };
}