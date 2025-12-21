using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Extensions;

namespace WellInsightEngine.Core.Features.WellInsights.FilterWellInsights;

public sealed class FilterWellInsightsFeature(IApplicationDbContext context)
{
    public async Task<FilterWellInsightsResponse> Handle(FilterWellInsightsRequest request, CancellationToken cancellation)
    {
        var query =
            context.WellInsights
                .AsNoTracking();

        if (request.Filter is not null)
        {
            var fromUtc = request.Filter.From.ToUniversalTime();
            var toUtc = request.Filter.To.ToUniversalTime();

            query = query.Where(x =>
                x.WellId == request.Filter.WellId &&
                x.CreatedAt >= fromUtc &&
                x.CreatedAt <= toUtc);
        }

        var projected = query
            .OrderByDescending(x => x.CreatedAt)
            .ProjectToResponse();

        var paged = await projected.ToOffsetPagedListAsync(request.Pagination, cancellation);
        return FilterWellInsightsResponse.Create(paged);
    }
}