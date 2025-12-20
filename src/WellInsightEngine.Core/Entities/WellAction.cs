using WellInsightEngine.Core.Enums;

namespace WellInsightEngine.Core.Entities;

public sealed class WellAction
{
    public Guid Id { get; init; }
    public Guid WellId { get; set; }
    public Well? Well { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public WellActionSource Source { get; set; }
}