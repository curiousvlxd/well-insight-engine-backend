using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.Insight;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class InsightActionConfiguration : IEntityTypeConfiguration<InsightAction>
{
    public void Configure(EntityTypeBuilder<InsightAction> e)
    {
        e.HasKey(x => new { x.InsightId, x.WellActionId });

        e.HasOne<Insight>()
            .WithMany()
            .HasForeignKey(x => x.InsightId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne<WellAction>()
            .WithMany()
            .HasForeignKey(x => x.WellActionId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasIndex(x => x.WellActionId);
    }
}