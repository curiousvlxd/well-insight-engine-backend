using Microsoft.EntityFrameworkCore;

namespace WellInsightEngine.Core.Features.GetWell;

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