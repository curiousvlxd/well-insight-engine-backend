using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Extensions;

namespace WellInsightEngine.Core.Features.WellInsights.FilterWellInsights;

public sealed class FilterWellInsightsFeature(IApplicationDbContext context)
{
    public async Task<FilterWellInsightsResponse> Handle(FilterWellInsightsRequest request, CancellationToken cancellation)
    {
        var fromUtc = request.From.ToUniversalTime();
        var toUtc = request.To.ToUniversalTime();

        var query =
            context.WellInsights
                .AsNoTracking()
                .Where(x =>
                    x.WellId == request.WellId &&
                    x.CreatedAt >= fromUtc &&
                    x.CreatedAt <= toUtc)
                .OrderByDescending(x => x.CreatedAt)
                .ProjectToResponse();

        var paged = await query.ToOffsetPagedListAsync(request.Pagination, cancellation);
        return FilterWellInsightsResponse.Create(paged);
    }
}