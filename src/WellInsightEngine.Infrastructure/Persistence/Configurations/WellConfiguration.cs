using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellInsightEngine.Core.Entities;

namespace WellInsightEngine.Infrastructure.Persistence.Configurations;

public sealed class WellConfiguration : IEntityTypeConfiguration<Well>
{
    public void Configure(EntityTypeBuilder<Well> e)
    {
        e.HasKey(x => x.Id);

        e.Property(x => x.AssetId).IsRequired();
        e.Property(x => x.Name).HasMaxLength(200).IsRequired();
        e.Property(x => x.ExternalId).HasMaxLength(200).IsRequired();

        e.HasOne(x => x.Asset)
            .WithMany(x => x.Wells)
            .HasForeignKey(x => x.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasMany(x => x.Parameters)
            .WithOne(x => x.Well)
            .HasForeignKey(x => x.WellId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasMany(x => x.Actions)
            .WithOne(x => x.Well)
            .HasForeignKey(x => x.WellId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasIndex(x => new { x.AssetId, x.Name });
        e.HasIndex(x => x.ExternalId);
    }
}