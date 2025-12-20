using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Extensions;

namespace WellInsightEngine.Core.Features.WellActions.FilterWellActions;

public sealed class FilterWellActionsFeature(IApplicationDbContext context)
{
    public async Task<FilterWellActionsResponse> Handle(FilterWellActionsRequest request, CancellationToken cancellation)
    {
        var query =
            context.WellActions
                .AsNoTracking()
                .Where(x =>
                    x.WellId == request.WellId &&
                    x.Timestamp >= request.FromUtc &&
                    x.Timestamp <= request.ToUtc)
                .OrderByDescending(x => x.Timestamp)
                .ProjectToResponse();
        var paged = await query.ToOffsetPagedListAsync(request.Pagination, cancellation);
        return FilterWellActionsResponse.Create(paged);
    }
}