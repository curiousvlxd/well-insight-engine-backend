using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.WellInsight.Payload;
using WellInsightEngine.Core.Features.WellMetrics;

namespace WellInsightEngine.Core.Abstractions.Services.WellInsightsAi.Contracts;

public sealed record GenerateWellInsightAiRequest
{
    public required Well Well { get; init; }
    public required DateTimeOffset FromUtc { get; init; }
    public required DateTimeOffset ToUtc { get; init; }
    public required GroupingInterval Interval { get; init; }
    public required WellInsightPayload Payload { get; init; }
    public required List<WellAction> Actions { get; init; }

    public static GenerateWellInsightAiRequest Create(Well well, DateTimeOffset fromUtc, DateTimeOffset toUtc, GroupingInterval interval, WellInsightPayload payload, IEnumerable<WellAction> actions)
        => new()
        {
            Well = well,
            FromUtc = fromUtc,
            ToUtc = toUtc,
            Interval = interval,
            Payload = payload,
            Actions = actions.ToList()
        };
    
    public void Deconstruct(out WellInsightPayload payload, out List<WellAction> actions, out DateTimeOffset fromUtc, out GroupingInterval interval, out DateTimeOffset toUtc, out Well well)
    {
        payload = Payload;
        actions = Actions;
        fromUtc = FromUtc;
        interval = Interval;
        toUtc = ToUtc;
        well = Well;
    }
}