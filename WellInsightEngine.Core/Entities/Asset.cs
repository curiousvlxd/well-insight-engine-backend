namespace WellInsightEngine.Core.Entities;

public sealed class Asset
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public Asset? Parent { get; set; }
    public List<Asset> Children { get; } = [];
    public List<Well> Wells { get; } = [];
}