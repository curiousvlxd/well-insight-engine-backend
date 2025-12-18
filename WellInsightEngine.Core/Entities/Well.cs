namespace WellInsightEngine.Core.Entities;

public sealed class Well
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public Guid AssetId { get; set; }
    public Asset? Asset { get; set; } 
    public string ExternalId { get; set; } = string.Empty;
    public List<WellParameter> Parameters { get; } = [];
    public List<WellAction> Actions { get; } = [];
}