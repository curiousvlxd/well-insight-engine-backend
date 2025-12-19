using Riok.Mapperly.Abstractions;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Core.Features.GetAssets;

[Mapper]
public static partial class GetAssetsMapper
{
    public static partial IQueryable<AssetResponse> ProjectToResponse(this IQueryable<Asset> q);
    private static partial AssetResponse Map(Asset asset);
    private static partial WellResponse Map(Well well);
}