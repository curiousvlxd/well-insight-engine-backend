using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Entities.Insight.Payload;

public sealed record InsightEvent
{
    public DateTimeOffset Timestamp { get; init; }
    public string Title { get; init; }
    public string Details { get; init; }
    public WellActionSource Source { get; init; }
}