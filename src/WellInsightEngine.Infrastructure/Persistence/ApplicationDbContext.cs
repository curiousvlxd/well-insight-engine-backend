using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.WellInsight;

namespace WellInsightEngine.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Well> Wells => Set<Well>();
    public DbSet<Parameter> Parameters => Set<Parameter>();
    public DbSet<WellParameter> WellParameters => Set<WellParameter>();
    public DbSet<WellAction> WellActions => Set<WellAction>();
    public DbSet<WellInsight> WellInsights => Set<WellInsight>();
    public DbSet<WellInsightAction> WellInsightActions => Set<WellInsightAction>();
    void IApplicationDbContext.Add<TEntity>(TEntity entity) => Set<TEntity>().Add(entity);
    void IApplicationDbContext.AddRange<TEntity>(List<TEntity> entities) => Set<TEntity>().AddRange(entities);
    async Task IApplicationDbContext.BulkInsertAsync<TEntity>(List<TEntity> entities, CancellationToken cancellation)
        where TEntity : class
    {
        if (entities.Count == 0)
            return;

        await this.BulkInsertAsync(entities, cancellationToken: cancellation);
    }
    IQueryable<Asset> IApplicationDbContext.Assets => Assets;
    IQueryable<Well> IApplicationDbContext.Wells => Wells;
    IQueryable<WellInsightAction> IApplicationDbContext.WellInsightActions => Set<WellInsightAction>();
    IQueryable<Parameter> IApplicationDbContext.Parameters => Parameters;
    IQueryable<WellParameter> IApplicationDbContext.WellParameters => WellParameters;
    IQueryable<WellAction> IApplicationDbContext.WellActions => WellActions;
    IQueryable<WellInsight> IApplicationDbContext.WellInsights => WellInsights;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}