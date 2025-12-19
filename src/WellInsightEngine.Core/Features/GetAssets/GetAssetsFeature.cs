using Microsoft.EntityFrameworkCore;

namespace WellInsightEngine.Core.Features.GetAssets;

public sealed class GetAssetsFeature(IApplicationDbContext context)
{
    public async Task<GetAssetsResponse> Handle(CancellationToken ct)
    {
        var assets = await context.Assets
            .AsNoTracking()
            .ProjectToResponse()
            .ToListAsync(ct);
        return GetAssetsResponse.Create(assets);
    }
}