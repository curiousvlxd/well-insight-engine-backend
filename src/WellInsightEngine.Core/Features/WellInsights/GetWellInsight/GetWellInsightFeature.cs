using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Features.WellInsights.Common;

namespace WellInsightEngine.Core.Features.WellInsights.GetWellInsight;

public sealed class GetWellInsightFeature(IApplicationDbContext context)
{
    public async Task<WellInsightResponse?> Handle(GetWellInsightRequest request, CancellationToken cancellation)
    {
        var response = await context.WellInsights
            .AsNoTracking()
            .Where(x => x.Slug == request.Slug)
            .ProjectToResponse()
            .FirstOrDefaultAsync(cancellation);
        return response;
    }
}