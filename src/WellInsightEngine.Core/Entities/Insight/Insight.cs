namespace WellInsightEngine.Core.Entities.Insight;

public sealed class Insight
{
    public Guid Id { get; init; }
    public Guid WellId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
}