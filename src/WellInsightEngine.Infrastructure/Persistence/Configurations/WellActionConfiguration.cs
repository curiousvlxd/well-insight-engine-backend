using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class WellActionConfiguration : IEntityTypeConfiguration<WellAction>
{
    public void Configure(EntityTypeBuilder<WellAction> e)
    {
        e.HasKey(x => x.Id);

        e.Property(x => x.WellId).IsRequired();
        e.Property(x => x.Timestamp).IsRequired();

        e.Property(x => x.Type).HasConversion<short>().IsRequired();
        e.Property(x => x.Source).HasConversion<short>().IsRequired();

        e.Property(x => x.Title).HasMaxLength(300).IsRequired();
        e.Property(x => x.Details).HasMaxLength(8000).IsRequired();

        e.HasOne(x => x.Well)
            .WithMany(x => x.Actions)
            .HasForeignKey(x => x.WellId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasIndex(x => new { x.WellId, x.Timestamp });
        e.HasIndex(x => new { x.WellId, x.Type, x.Timestamp });
    }
}