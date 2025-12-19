using Microsoft.EntityFrameworkCore;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Well> Wells => Set<Well>();
    public DbSet<Parameter> Parameters => Set<Parameter>();
    public DbSet<WellParameter> WellParameters => Set<WellParameter>();
    public DbSet<WellAction> WellActions => Set<WellAction>();
    public DbSet<Insight> Insights => Set<Insight>();
    public DbSet<InsightAction> InsightActions => Set<InsightAction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
}