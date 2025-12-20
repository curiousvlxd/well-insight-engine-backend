using Microsoft.EntityFrameworkCore;

namespace WellInsightEngine.Core.Features.GetAssets;

public sealed class GetAssetsFeature(IApplicationDbContext context)
{
    public async Task<GetAssetsResponse> Handle(CancellationToken cancellation)
    {
        var assets = await context.Assets
            .AsNoTracking()
            .ProjectToResponse()
            .ToListAsync(cancellation);
        return GetAssetsResponse.Create(assets);
    }
}