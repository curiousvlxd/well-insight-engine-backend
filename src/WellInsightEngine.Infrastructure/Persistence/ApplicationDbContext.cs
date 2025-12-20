using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Abstractions.Persistence;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.Insight;

namespace WellInsightEngine.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Well> Wells => Set<Well>();
    public DbSet<Parameter> Parameters => Set<Parameter>();
    public DbSet<WellParameter> WellParameters => Set<WellParameter>();
    public DbSet<WellAction> WellActions => Set<WellAction>();
    public DbSet<Insight> Insights => Set<Insight>();
    public DbSet<InsightAction> InsightActions => Set<InsightAction>();
    void IApplicationDbContext.Add<TEntity>(TEntity entity) => Set<TEntity>().Add(entity);
    IQueryable<Asset> IApplicationDbContext.Assets => Assets;
    IQueryable<Well> IApplicationDbContext.Wells => Wells;
    IQueryable<Parameter> IApplicationDbContext.Parameters => Parameters;
    IQueryable<WellParameter> IApplicationDbContext.WellParameters => WellParameters;
    IQueryable<WellAction> IApplicationDbContext.WellActions => WellActions;
    IQueryable<Insight> IApplicationDbContext.Insights => Insights;
    IQueryable<InsightAction> IApplicationDbContext.InsightActions => InsightActions;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
}
