using Microsoft.EntityFrameworkCore.Infrastructure;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.WellInsight;

namespace WellInsightEngine.Core.Abstractions.Persistence;

public interface IApplicationDbContext
{
    IQueryable<Asset> Assets { get; }
    IQueryable<Well> Wells { get; }
    IQueryable<Parameter> Parameters { get; }
    DatabaseFacade Database { get; }
    IQueryable<WellParameter> WellParameters { get; }
    IQueryable<WellInsightAction> WellInsightActions { get; }
    IQueryable<WellAction> WellActions { get; }
    IQueryable<WellInsight> WellInsights { get; }
    void Add<TEntity>(TEntity entity) where TEntity : class;
    void AddRange<TEntity>(List<TEntity> entitis) where TEntity : class;
    Task BulkInsertAsync<TEntity>(List<TEntity> entities, CancellationToken cancellation = default) where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}