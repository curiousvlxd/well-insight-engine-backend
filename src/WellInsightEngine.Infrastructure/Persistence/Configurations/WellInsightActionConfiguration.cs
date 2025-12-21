using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellInsightEngine.Core.Entities;
using WellInsightEngine.Core.Entities.WellInsight;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class WellInsightActionConfiguration : IEntityTypeConfiguration<WellInsightAction>
{
    public void Configure(EntityTypeBuilder<WellInsightAction> e)
    {
        e.HasKey(x => new { x.InsightId, x.WellActionId });

        e.HasOne<WellInsight>()
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