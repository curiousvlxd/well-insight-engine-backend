namespace WellInsightEngine.Core.Entities;

public sealed class WellInsightAction
{   
    public WellInsight.WellInsight? Insight { get; set; }
    public Guid InsightId { get; set; }
    public WellAction? WellAction { get; set; }
    public Guid WellActionId { get; set; }

    public static WellInsightAction Create(Guid insightId, Guid wellActionId) => new()
    {
        InsightId = insightId,
        WellActionId = wellActionId
    };
}