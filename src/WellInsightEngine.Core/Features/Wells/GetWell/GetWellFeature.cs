using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;

namespace WellInsightEngine.Core.Features.Wells.GetWell;

public sealed class GetWellFeature(IApplicationDbContext context)
{
    public async Task<GetWellResponse?> Handle(Guid wellId, CancellationToken ct)
    {
        return await context.Wells
            .AsNoTracking()
            .Where(w => w.Id == wellId)
            .ProjectToResponse()
            .FirstOrDefaultAsync(ct);
    }
}