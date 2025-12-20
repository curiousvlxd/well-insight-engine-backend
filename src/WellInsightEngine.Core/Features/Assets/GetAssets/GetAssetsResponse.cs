namespace WellInsightEngine.Core.Features.Assets.GetAssets;

public sealed class GetAssetsResponse
{
    public List<AssetResponse> Assets { get; set; } = [];

    public static GetAssetsResponse Create(List<AssetResponse> assets)
    {
        return new GetAssetsResponse
        {
            Assets = assets
        };
    }
}

public sealed class AssetResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid? ParentId { get; init; }
    public List<WellResponse> Wells { get; init; } = [];
}

public sealed class WellResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ExternalId { get; init; } = string.Empty;
}
