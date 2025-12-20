using Microsoft.EntityFrameworkCore.Infrastructure;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.Insight;

namespace WellInsightEngine.Core.Abstractions.Persistence;

public interface IApplicationDbContext
{
    IQueryable<Asset> Assets { get; }
    IQueryable<Well> Wells { get; }
    IQueryable<Parameter> Parameters { get; }
    DatabaseFacade Database { get; }
    IQueryable<WellParameter> WellParameters { get; }
    IQueryable<WellAction> WellActions { get; }
    IQueryable<Insight> Insights { get; }
    IQueryable<InsightAction> InsightActions { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}