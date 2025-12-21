using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Extensions;

namespace WellInsightEngine.Core.Features.WellInsightActions;

public sealed class FilterWellInsightActionsFeature(IApplicationDbContext context)
{
    public async Task<FilterWellInsightActionsResponse> Handle(FilterWellInsightActionsRequest request, CancellationToken cancellation)
    {
        var query =
            context.WellInsightActions
                .AsNoTracking()
                .Where(x => x.InsightId == request.InsightId && x.WellAction != null)
                .Select(x => x.WellAction)
                .OfType<WellAction>();

        if (request.Range is not null)
        {
            var fromUtc = request.Range.From.ToUniversalTime();
            var toUtc = request.Range.To.ToUniversalTime();

            query = query.Where(x =>
                x.Timestamp >= fromUtc &&
                x.Timestamp <= toUtc);
        }

        var projected = query
            .OrderByDescending(x => x.Timestamp)
            .ProjectToResponse();

        var paged = await projected.ToOffsetPagedListAsync(request.Pagination, cancellation);
        return FilterWellInsightActionsResponse.Create(paged);
    }
}