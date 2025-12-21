namespace WellInsightEngine.Core.Entities;

public sealed class WellInsightAction
{
    public Guid InsightId { get; set; }
    public Guid WellActionId { get; set; }

    public static WellInsightAction Create(Guid insightId, Guid wellActionId) => new()
    {
        InsightId = insightId,
        WellActionId = wellActionId
    };
}